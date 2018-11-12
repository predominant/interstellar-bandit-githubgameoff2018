using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

namespace ghg2018
{
	public class ThirdPersonShipControl : MonoBehaviour
	{
		private Rigidbody _rigidbody;

		[SerializeField]
		private float _maxVelocity = 10f;

		[SerializeField]
		private float _verticalSpeed = 0.05f;

		[SerializeField]
		private float _rotationSmooth = 0.1f;

		private Quaternion _targetRotation;

		[SerializeField]
		private GameObject _explosion;

		[SerializeField]
		private GameObject[] _renderer;

		private bool _controllable = true;

		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody>();
			this._targetRotation = this.transform.localRotation;
		}

		private void Update()
		{
			if (!this._controllable)
				return;
			
			//this.transform.rotation = Quaternion.AngleAxis(Input.GetAxis("Horizontal"), this.transform.up);
			this._targetRotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Horizontal"));
			this.transform.localRotation = Quaternion.Lerp(this.transform.rotation, this._targetRotation, this._rotationSmooth);

			var velocity = this._rigidbody.velocity;
			var vertVelocity = velocity.y;
			velocity.y = 0f;
			
			velocity += this.transform.forward * Input.GetAxis("Vertical");
			velocity = Mathf.Clamp(
			   velocity.magnitude,
			   0f, this._maxVelocity) * this.transform.forward;

			var verticalVelocity = Vector3.up * Input.GetAxis("Jump") * this._verticalSpeed -
			                       Vector3.up * Input.GetAxis("Crouch") * this._verticalSpeed;
			this._rigidbody.velocity = velocity + verticalVelocity;
		}

		private void OnCollisionEnter(Collision other)
		{
			this._explosion.SetActive(true);
			foreach (var o in this._renderer)
				o.SetActive(false);
			this._controllable = false;
		}
	}
}