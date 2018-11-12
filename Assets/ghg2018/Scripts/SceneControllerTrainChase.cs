using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public enum TrainSceneMode
	{
		Scan,
		Align
	}
	
	public class SceneControllerTrainChase : MonoBehaviour
	{
		private string _nextScene = "cutscene-2";
		
		[SerializeField]
		private float _trainCargoProbability = 0.2f;

		private int _numberOfTrainCars = 0;

		public int ScannedCars = 0;

		[SerializeField]
		private GameObject _scanArea;

		[SerializeField]
		private GameObject _alignArea;

		public bool Aligned
		{
			get { return false; }
			set
			{
				if (value)
					this.BoardTrain();
			}
		}

		public TrainSceneMode Mode = TrainSceneMode.Scan;
		
		private void Awake()
		{
			var trainCars = GameObject.FindObjectsOfType<TrainCar>();
			this._numberOfTrainCars = trainCars.Length;
			
			var numCargo = 0;
			while (numCargo < 1)
			{
				foreach (var car in trainCars)
				{
					if (Random.Range(0f, 1f) < this._trainCargoProbability)
					{
						car.HasCargo = true;
						numCargo++;
					}
				}
			}
			
			Debug.Log(string.Format("We have {0} train cars with cargo", numCargo));
		}

		private void Update()
		{
			if (this._numberOfTrainCars == 0)
				return;

			if (this._numberOfTrainCars == this.ScannedCars)
			{
				this.SwitchMode();
			}
		}

		private void SwitchMode()
		{
			this.Mode = TrainSceneMode.Align;
			this._scanArea.SetActive(false);
			this._alignArea.SetActive(true);
		}

		private void BoardTrain()
		{
			Debug.Log("Boarding train");
			SceneManager.LoadScene(this._nextScene);
		}
	}
}