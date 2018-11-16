using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class SideViewTerrainScroller : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _terrains;

		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private Vector3 _movementDirection = Vector3.left;

		private void Update()
		{
			foreach (var t in this._terrains)
			{
				t.Translate(this._movementDirection * Time.deltaTime * this._speed);
				if (t.position.x < -750f)
					t.Translate(this._movementDirection * -1f * 500f * 2f);
			}
		}
	}
}