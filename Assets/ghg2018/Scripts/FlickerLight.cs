using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(Light))]
	public class FlickerLight : MonoBehaviour
	{
		[SerializeField]
		private float _delay = 3.5f;
		[SerializeField]
		private float _splay = 1.5f;
		[SerializeField]
		private float _offTimer = 0.1f;

		[SerializeField]
		private Color _onColor;
		[SerializeField]
		private Color _offColor;

		private float _nextFlicker = 0f;

		private Light _light;
		
		
		private void Awake()
		{
			this._light = this.GetComponent<Light>();
			this.SetNextFlicker();
		}
		
		private void Update()
		{
			if (this._nextFlicker == 0f || Time.time > this._nextFlicker)
			{
				if (Time.time < this._nextFlicker + this._offTimer)
				{
					this.Switch(false);
				}
				else
				{
					this.Switch(true);
					this.SetNextFlicker();
				}
			}
		}

		private void SetNextFlicker()
		{
			this._nextFlicker = Time.time + Random.Range(this._delay - (this._splay / 2f), this._delay + (this._splay / 2f));
		}
		
		private void Switch(bool value)
		{
			this._light.color = value ? this._onColor : this._offColor;
		}
	}
}