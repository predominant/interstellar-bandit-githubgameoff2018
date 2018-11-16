using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class CameraFollow2d : MonoBehaviour
	{
		[SerializeField]
		private Transform _target;

		private void Update()
		{
			var pos = this.transform.position;
			pos.x = this._target.position.x;
			this.transform.position = pos;
		}
	}
}