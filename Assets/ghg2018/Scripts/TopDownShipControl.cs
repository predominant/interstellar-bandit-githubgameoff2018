using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(Rigidbody))]
	public class TopDownShipControl : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		[SerializeField]
		private float SpeedMultiplier = 10f;

		[SerializeField]
		private float RotationMultiplier = 1f;

		[Header("Thruster Settings")]
		[SerializeField]
		private ParticleSystem[] EngineParticles;

		private float ThrusterRestLifetime = 0.2f;
		private float ThrusterActiveLifetime = 0.45f;
		
		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody>();
		}

		private void Update()
		{
			var velocity = Input.GetAxis("Vertical");
			this._rigidbody.velocity = this.transform.forward * velocity * this.SpeedMultiplier;

			var rotation = Input.GetAxis("Horizontal");
			this.transform.Rotate(this.transform.up, rotation * this.RotationMultiplier * Time.deltaTime);

			foreach (var ps in this.EngineParticles)
				if (Mathf.Abs(velocity) > 0.1f)
					ps.startLifetime = this.ThrusterActiveLifetime;
				else
					ps.startLifetime = this.ThrusterRestLifetime;
		}
	}
}