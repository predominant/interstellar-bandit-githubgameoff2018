using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerTrainRobbing : MonoBehaviour
	{
		[Serializable]
		private enum SceneControllerTrainRobbingState
		{
			Nothing,
			InspectCrate,
			ExitTrain,
			Decouple,
		}

		private SceneControllerTrainRobbingState _sceneState = SceneControllerTrainRobbingState.Nothing;

		private SceneControllerTrainRobbingState SceneState
		{
			get { return this._sceneState; }
			set
			{
				this._sceneState = value;
				switch (this._sceneState)
				{
					case SceneControllerTrainRobbingState.InspectCrate:
						this._cratePickupMessagePanel.SetActive(true);
						this._exitTrainMessagePanel.SetActive(false);
						break;
					case SceneControllerTrainRobbingState.ExitTrain:
						this._cratePickupMessagePanel.SetActive(false);
						this._exitTrainMessagePanel.SetActive(true);
						break;
//					case SceneControllerTrainRobbingState.Decouple:
//						this._cratePickupMessagePanel.SetActive(false);
//						this._exitTrainMessagePanel.SetActive(false);
//						// TODO: This would be SUPER cool - Be able to decouple the train cars at will
//						break;
					case SceneControllerTrainRobbingState.Nothing:
					default:
						this._cratePickupMessagePanel.SetActive(false);
						this._exitTrainMessagePanel.SetActive(false);
						break;
				}
			}
		}
		
		[SerializeField]
		private PlayerController2d _playerController;
		
		[SerializeField]
		private float _sceneDuration = 5f * 60f;
		private float _startTime;

		[SerializeField]
		private TextMeshProUGUI _timerText;
		[SerializeField]
		private TextMeshProUGUI _haulText;
		[SerializeField]
		private GameObject _cratePickupMessagePanel;
		[SerializeField]
		private GameObject _exitTrainMessagePanel;

		[SerializeField]
		private GameObject _failedPanel;

		private CargoContainer _targetCargo;

		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private AudioClip _15secondsRemainClip;
		private bool _played15remaining = false;
		[SerializeField]
		private AudioClip _10secondsRemainClip;
		private bool _played10remaining = false;
		[SerializeField]
		private AudioClip _5secondsRemainClip;
		private bool _played5remaining = false;
		[SerializeField]
		private AudioClip _failedSceneClip;
		private bool _playedFailed = false;

		private bool _exiting = false;

		private int _haulItems = 0;
		public int HaulItems
		{
			get { return this._haulItems; }
			private set
			{
				this._haulItems = value;
				this.UpdateHaulUI();
			}
		}

		private int _haulValue = 0;
		public int HaulValue
		{
			get { return this._haulValue; }
			private set
			{
				this._haulValue = value;
				this.UpdateHaulUI();
			}
		}

		private void Awake()
		{
			this._startTime = Time.time;

			this.UpdateHaulUI();
			this._playerController.OnCargoInRange += this.ShowCratePickupMessage;
			this._playerController.OnCargoOutOfRange += this.HideCratePickupMessage;
			this._playerController.OnExtractionInRange += this.ShowExtractionMessage;
			this._playerController.OnExtractionOutOfRange += this.HideExtractionMessage;
		}

		private void OnDestroy()
		{
			this._playerController.OnCargoInRange -= this.ShowCratePickupMessage;
			this._playerController.OnCargoOutOfRange -= this.HideCratePickupMessage;
			this._playerController.OnExtractionInRange -= this.ShowExtractionMessage;
			this._playerController.OnExtractionOutOfRange -= this.HideExtractionMessage;
		}

		private void Update()
		{
			if (this.TimeRanOut() && !this._exiting)
				this.FailScene();

			this.UpdateTimer();

			this.ProcessControls();

			this.VoiceMessages();
		}

		private void ProcessControls()
		{
			if (Input.GetAxis("Jump") > 0.001f)
				this.Action();
		}

		private bool TimeRanOut()
		{
			return Time.time - this._startTime > this._sceneDuration;
		}

		public void FailScene()
		{
			this._played5remaining = true;
			this._played10remaining = true;
			this._played15remaining = true;
			this._audioSource.Pause();
			this._audioSource.clip = this._failedSceneClip;
			this._audioSource.Play();

			this._failedPanel.SetActive(true);
			this._playerController.Controllable = false;
		}

		public void Retry()
		{
			// Just reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void Quit()
		{
			// Load back into the main menu
			SceneManager.LoadScene("menu");
		}

		private void UpdateTimer()
		{
			var totalSeconds = this.TimeRemaining();
			var seconds = totalSeconds % 60;
			var minutes = totalSeconds / 60;
			this._timerText.text = string.Format("Time: {0}:{1:00}", minutes, seconds);
		}

		private int TimeRemaining()
		{
			return Mathf.FloorToInt(Mathf.Clamp(
				this._sceneDuration - (Time.time - this._startTime),
				0f,
				this._sceneDuration));
		}

		private void ShowCratePickupMessage(CargoContainer c)
		{
			this._targetCargo = c;
			this.SceneState = SceneControllerTrainRobbingState.InspectCrate;
		}

		private void HideCratePickupMessage()
		{
			this._cratePickupMessagePanel.SetActive(false);
			this._targetCargo = null;
		}

		private void ShowExtractionMessage()
		{
//			this._exitTrainMessagePanel.SetActive(true);
			this.SceneState = SceneControllerTrainRobbingState.ExitTrain;
		}

		private void HideExtractionMessage()
		{
//			this._exitTrainMessagePanel.SetActive(false);
			this.SceneState = SceneControllerTrainRobbingState.Nothing;
		}

		private void UpdateHaulUI()
		{
			this._haulText.text = string.Format(
				"Value: {0}\nItems: {1}",
				string.Format("{0:n0}", this.HaulValue),
				this.HaulItems);
		}

		private void Action()
		{
			if (!this._playerController.Controllable)
				return;
			
			switch (this._sceneState)
			{
				case SceneControllerTrainRobbingState.InspectCrate:
					if (this._targetCargo != null && this._targetCargo.CanLoot())
					{
						this._cratePickupMessagePanel.SetActive(false);
						this.HaulValue += this._targetCargo.Loot();
						this.HaulItems++;
					}
					break;
				case SceneControllerTrainRobbingState.ExitTrain:
					this.ExitTrain();
					break;
				case SceneControllerTrainRobbingState.Nothing:
				default:
					// Do nothing
					break;
			}
		}

		private void ExitTrain()
		{
			this._exiting = true;
			SceneManager.LoadScene("cutscene-4");
		}

		private void VoiceMessages()
		{
			var timeRemaining = this._sceneDuration - (Time.time - this._startTime);

			if (timeRemaining <= 19f && !this._played15remaining)
			{
				this._played15remaining = true;
				this._audioSource.clip = this._15secondsRemainClip;
				this._audioSource.Play();
			}

			if (timeRemaining <= 14f && !this._played10remaining)
			{
				this._played10remaining = true;
				this._audioSource.clip = this._10secondsRemainClip;
				this._audioSource.Play();
			}

			if (timeRemaining <= 5.75f && !this._played5remaining)
			{
				this._played5remaining = true;
				this._audioSource.clip = _5secondsRemainClip;
				this._audioSource.Play();
			}
		}
	}
}