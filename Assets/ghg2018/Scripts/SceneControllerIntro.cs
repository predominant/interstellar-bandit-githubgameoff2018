using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerIntro : MonoBehaviour
	{
		[SerializeField]
		private AudioClip _narratorClip;

		[SerializeField]
		private float _narratorStartTime;

		private bool _narratorPlayed = false;

		private AudioSource _audioSource;

		[SerializeField]
		private float _sceneChangeTime = 75f;

		[SerializeField]
		private string _nextScene = "cutscene-1";

		private bool _loadedNextScene = false;
		private bool _activatedWarp = false;

		private void Start()
		{
			this._audioSource = this.gameObject.AddComponent<AudioSource>();
		}
		
		private void Update()
		{
			if (!this._activatedWarp && Time.timeSinceLevelLoad >= 0.2f)
			{
				this._activatedWarp = true;
				this.GetComponent<WarpOnOff>().Activate();
			}
			
			if (!this._narratorPlayed && Time.timeSinceLevelLoad >= this._narratorStartTime)
			{
				this._narratorPlayed = true;
				this._audioSource.clip = this._narratorClip;
				this._audioSource.Play();
			}

			if (!this._loadedNextScene && Time.timeSinceLevelLoad >= this._sceneChangeTime)
			{
				this._loadedNextScene = true;
				SceneManager.LoadScene(this._nextScene);
			}
		}
	}
}