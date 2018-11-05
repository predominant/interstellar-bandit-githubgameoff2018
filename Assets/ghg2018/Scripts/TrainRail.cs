using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(BGCurve))]
	public class TrainRail : MonoBehaviour
	{
		[SerializeField]
		private GameObject RailPrefab;

		private GameObject[] _rails = new GameObject[0];
		
		private BGCurve _curve;
		private BGCcMath _bgMath;

		private void Awake()
		{
			if (this._bgMath == null)
				this._bgMath = this.GetComponent<BGCcMath>();
			
			if (this._curve == null)
				this._curve = this.GetComponent<BGCurve>();

			this._curve.Changed += this.RenderRails;
			this.RenderRails(null, null);
		}
		
		private void Update()
		{
		}

		private void RenderRails(object o, BGCurveChangedArgs args)
		{
			var splitter = this.GetComponent<BGCcSplitterPolyline>();

			// Delete all existing rails
			for (var i = 0; i < this._rails.Length; i++)
				GameObject.Destroy(this._rails[i]);
			
			// Create new rails
			this._rails = new GameObject[splitter.PointsCount];
			for (var i = 0; i < splitter.PointsCount; i++)
			{
				var tangent = splitter.Math.CalcTangentByDistanceRatio((float)i / (float)splitter.PointsCount);
				this._rails[i] = GameObject.Instantiate(
					this.RailPrefab,
					this.transform.position + splitter.Positions[i],
					Quaternion.LookRotation(tangent),
					this.transform);
				
			}
		}

		public Vector3 PositionByRatio(float value)
		{
			return this._bgMath.CalcPositionByDistanceRatio(value);
		}

		public Vector3 TangentByRatio(float value)
		{
			return this._bgMath.CalcTangentByDistanceRatio(value);
		}
	}
}