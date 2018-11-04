using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace ghg2018
{
	public class EnemyShip : MonoBehaviour
	{
		[SerializeField]
		private float ScanRange = 25f;

		[SerializeField]
		private float SpeedMultiplier = 7f;

		[SerializeField]
		private float FlightRange = 200f;

		[SerializeField]
		private float Bounds = 900f;

		[SerializeField]
		private float RotateSpeed = 1f;

		[SerializeField]
		private float ShotDelay = 0.5f;
		[SerializeField]
		private GameObject[] PhaserPrefabs;
		[SerializeField]
		private Transform[] PhaserPoints;
		[SerializeField]
		private float PhaserLifetime = 3f;
		[SerializeField]
		private float PhaserForce = 1f;
		private float LastShot = 0;

		private Transform Player;
		private bool FollowingPlayer;

		private Vector3 Target;
		private Rigidbody _rigidbody;

		private bool HasTarget
		{
			get
			{
				return this.Target != null && this.Target != Vector3.zero &&
				       Vector3.Distance(this.transform.position, this.Target) > 1f;
			}
		}

		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody>();
			this.Player = GameObject.FindWithTag("Player").transform;
		}

		private void Update()
		{
			this.TrackTarget();
			this.ShootTarget();
		}

		private void TrackTarget()
		{
			if (this.FollowingPlayer)
			{
				if (!this.DetectPlayer())
				{
					this.FollowingPlayer = false;
					this.Target = Vector3.zero;
					return;
				}

				this.Target = this.Player.position;
			}

			if (!this.HasTarget)
				this.Target = this.RandomPatrolPoint();
		
			if (this.HasTarget)
			{
				this.transform.rotation = Quaternion.RotateTowards(
					this.transform.rotation,
					Quaternion.LookRotation(this.Target - this.transform.position),
					this.RotateSpeed * Time.deltaTime);
				this._rigidbody.velocity = this.transform.forward * this.SpeedMultiplier;
			}
			else
			{
				this._rigidbody.velocity = Vector3.zero;
			}
			
			var foundPlayer = this.DetectPlayer();
			if (foundPlayer)
			{
				this.FollowingPlayer = true;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(this.transform.position, this.ScanRange);

			if (this.HasTarget)
			{
				Gizmos.color = this.FollowingPlayer ? Color.red : Color.yellow;
				Gizmos.DrawLine(this.transform.position, this.Target);
			}
		}

		private bool DetectPlayer()
		{
			var colliders = Physics.OverlapSphere(
				this.transform.position,
				this.ScanRange,
				LayerMask.GetMask("Player"));

			if (colliders.Length == 0)
				return false;

			return true;
		}

		private Vector3 RandomPatrolPoint()
		{
			var point = Random.insideUnitCircle * this.FlightRange;
			var target = new Vector3(point.x, 0f, point.y) + this.transform.position;

			var obstructed = Physics.Raycast(
				this.transform.position,
				target - this.transform.position,
				point.magnitude,
				LayerMask.GetMask("Planet"));

			if (obstructed)
				target = this.RandomPatrolPoint();
			if (!this.TargetInBounds(target))
				target = this.RandomPatrolPoint();
			
			return target;
		}

		private bool TargetInBounds(Vector3 pos)
		{
			return Mathf.Abs(pos.x) < this.Bounds && Mathf.Abs(pos.z) < this.Bounds;
		}

		private void ShootTarget()
		{
			if (!this.FollowingPlayer)
				return;

			if (Time.time < this.LastShot + this.ShotDelay)
				return;

			this.LastShot = Time.time;

			var prefab = this.PhaserPrefabs[Random.Range(0, this.PhaserPrefabs.Length)];
			foreach (var t in this.PhaserPoints)
			{
				var obj = GameObject.Instantiate(prefab, t.position, t.rotation);
				GameObject.Destroy(obj, this.PhaserLifetime);
				obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * this.PhaserForce, ForceMode.Impulse);
			}
		}
	}
}