using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class CameraSmoothFollow : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		[SerializeField]
		private Vector3 _offset;

		[SerializeField]
		private float _smoothSpeed = 0.2f;

		private Vector3 _smoothPosition = Vector3.zero;

		private void Awake()
		{
			this._smoothPosition = this._target.position +
			                       (this._target.forward * this._offset.z) + (this._target.up * this._offset.y);
			this.transform.position = this._smoothPosition;
		}

		private void Update()
		{
			var desiredPosition = this._target.position +
			                      (this._target.forward * this._offset.z) + (this._target.up * this._offset.y);
			this._smoothPosition = Vector3.Lerp(this.transform.position, desiredPosition, this._smoothSpeed);
			this.transform.position = this._smoothPosition;
			this.transform.rotation = Quaternion.Lerp(
				this.transform.rotation,
				Quaternion.LookRotation(this._target.position - this.transform.position, Vector3.up), this._smoothSpeed);
//			this.transform.LookAt(this._target);
		}
	}
}