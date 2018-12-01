using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class TrainEngine : TrainCar
	{
		[SerializeField]
		private float _trackEndPosition = 14200f;
		private bool _doFail = false;
		
		protected override void Update()
		{
			base.Update();

			if (this.TrackPosition >= this._trackEndPosition && !this._doFail)
			{
				this._doFail = true;
				GameObject.Find("Scene Controller").GetComponent<SceneControllerTrainChase>().Fail();
			}
		}
	}
}