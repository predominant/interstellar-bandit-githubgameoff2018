using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class PlanetRotation : MonoBehaviour
	{
		[SerializeField]
		private float _rate = 30f;

		private void Update()
		{
			this.transform.Rotate(
				this.transform.up,
				this._rate * Time.deltaTime);
		}
	}
}