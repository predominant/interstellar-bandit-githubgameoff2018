using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class CameraBounce : MonoBehaviour
	{
		[SerializeField]
		private float _delay = 3.5f;
		[SerializeField]
		private float _splay = 1.5f;
		[SerializeField]
		private float _offTimer = 0.2f;
		private float _nextBounce = 0f;

		private Vector3 _startPos;
		[SerializeField]
		private Vector3 _bounceOffset = new Vector3(0, -0.1f, 0);

		private void Awake()
		{
			this._startPos = this.transform.position;
		}

		private void Update()
		{
			if (this._nextBounce == 0f || Time.time > this._nextBounce)
			{
				if (Time.time < this._nextBounce + this._offTimer)
				{
					this.Bounce(false);
				}
				else
				{
					this.Bounce(true);
					this.SetNextBounce();
				}
			}
		}

		private void SetNextBounce()
		{
			this._nextBounce = Time.time + Random.Range(this._delay - (this._splay / 2f), this._delay + (this._splay / 2f));
		}

		private void Bounce(bool pos)
		{
			this.transform.position = this._startPos + (pos ? Vector3.zero : this._bounceOffset);
		}
	}
}