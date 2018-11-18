using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(LensFlare))]
	public class WarpLensFlare : MonoBehaviour
	{
		[SerializeField]
		private float _lifetime = 1.2f;

		[SerializeField]
		private float _multiplier = 1f;

		[SerializeField]
		private AnimationCurve _intensity;

		private LensFlare _lensFlare;

		private float _startTime;

		private void Awake()
		{
			this._lensFlare = this.GetComponent<LensFlare>();
		}

		private void Start()
		{
			this._startTime = Time.time;
		}
		
		private void Update()
		{
			var lifetime = this._startTime + this._lifetime - Time.time;
			lifetime /= this._lifetime;
			this._lensFlare.brightness = this._intensity.Evaluate(lifetime);
		}
	}
}