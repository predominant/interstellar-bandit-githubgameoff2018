using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class TrainEngine : MonoBehaviour
	{
		[SerializeField]
		private TrainCar _trainCar;

		[SerializeField]
		private TrainRail _trainRail;

		[SerializeField]
		private float Speed = 1f;

		private float _trackPosition = 0f;

		private void FixedUpdate()
		{
			this._trackPosition += this.Speed * Time.fixedDeltaTime;
			if (this._trackPosition >= 1f)
				this._trackPosition -= 1f;
		}

		private void Update()
		{
			this.transform.position = this._trainRail.PositionByRatio(this._trackPosition);
			this.transform.rotation = Quaternion.LookRotation(this._trainRail.TangentByRatio(this._trackPosition));
		}
	}
}