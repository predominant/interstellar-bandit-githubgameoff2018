using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(Animator))]
	public class EnemyController2d : PersonController2d
	{
		private enum EnemyControllerState
		{
			Patrol,
			Chasing,
			Dead
		}
		
		private Animator _animator;

		private EnemyControllerState _state = EnemyControllerState.Patrol;

		private float _startX = 0f;
		[SerializeField]
		private float _roamSize = 2.5f;

		private float _patrolTargetX = 0f;
		private float _lastRoam = 0f;
		[SerializeField]
		private float _roamDelay = 5f;
		[SerializeField]
		private float _walkSpeed = 1.5f;

		private float _scanRange = 4f;
		private float _scanLostRange = 6f;

		private GameObject _player;

		private new void Awake()
		{
			base.Awake();
			this._animator = this.GetComponent<Animator>();
			this._startX = this.transform.position.x;
			this._patrolTargetX = this._startX;
			this._player = GameObject.Find("Player");
		}

		private void Update()
		{
			if (this._state == EnemyControllerState.Dead)
				return;
			
			if (this.ShouldRoam())
				this.Roam();

			this.FlipPlayer(this._patrolTargetX > this.transform.position.x);
			
			switch (this._state)
			{
				case EnemyControllerState.Patrol:
					this.Move();
					this.CheckForPlayer();
					break;
				case EnemyControllerState.Chasing:
					this._patrolTargetX = this._player.transform.position.x;
					this.Move();
					this.Shoot();
					this.CheckLostPlayer();
					break;
			}
		}

		private void Move()
		{
			var targetPos = this.transform.position;
			targetPos.x = this._patrolTargetX;
			if ((targetPos - this.transform.position).magnitude > 0.1f)
				this.transform.Translate((targetPos - this.transform.position).normalized * Time.deltaTime * this._walkSpeed);
		}

		private void CheckForPlayer()
		{
			var colliders = Physics.OverlapSphere(this.transform.position, this._scanRange);
			foreach (var c in colliders)
			{
				if (c.gameObject.name == "Player")
				{
					this._state = EnemyControllerState.Chasing;
					return;
				}
			}
		}

		private void CheckLostPlayer()
		{
			var colliders = Physics.OverlapSphere(this.transform.position, this._scanLostRange);
			foreach (var c in colliders)
			{
				if (c.gameObject.name == "Player")
					return;
			}

			this._patrolTargetX = this.transform.position.x;
			this._state = EnemyControllerState.Patrol;
		}

		private bool ShouldRoam()
		{
			return (Time.time > this._lastRoam + this._roamDelay) &&
			       Mathf.Abs(this.transform.position.x - this._patrolTargetX) < 0.1f;
		}

		private void Roam()
		{
			this._lastRoam = Time.time;
			this._state = EnemyControllerState.Patrol;
			var posx = this.transform.position.x;
			this._patrolTargetX = Random.Range(posx - this._roamSize, posx + this._roamSize);
		}
		
		protected override void Die()
		{
			this._animator.SetBool("Dead", true);
			this._state = EnemyControllerState.Dead;
		}
	}
}