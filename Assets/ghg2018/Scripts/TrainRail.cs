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

		[SerializeField]
		public TrainRail PreviousTrack;
		[SerializeField]
		public TrainRail NextTrack;

		[SerializeField]
		private float _renderDistance = 20f;

		private GameObject[] _rails = new GameObject[0];
		
		private BGCurve _curve;
		private BGCcMath _bgMath;
		private BGCcCursor _bgCursor;

		private void Awake()
		{
			if (this._bgMath == null)
				this._bgMath = this.GetComponent<BGCcMath>();
			
			if (this._curve == null)
				this._curve = this.GetComponent<BGCurve>();

			if (this._bgCursor == null)
				this._bgCursor = this.GetComponent<BGCcCursor>();

			this.RenderRails(null, null);
		}
		
		private void RenderRails(object o, BGCurveChangedArgs args)
		{
			var distance = 0f;
			var rails = new List<GameObject>();
			while (distance < this._bgMath.GetDistance())
			{
				distance += this._renderDistance;
				rails.Add(GameObject.Instantiate(
					this.RailPrefab,
					this.PositionByDistance(distance),
					Quaternion.LookRotation(this.TangentByDistance(distance)),
					this.transform));
			}
			this._rails = rails.ToArray();
		}

		public Vector3 PositionByDistance(float value)
		{
			this._bgCursor.Distance = value;
			return this._bgCursor.CalculatePosition();
		}

		public Vector3 TangentByDistance(float value)
		{
			this._bgCursor.Distance = value;
			return this._bgCursor.CalculateTangent();
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