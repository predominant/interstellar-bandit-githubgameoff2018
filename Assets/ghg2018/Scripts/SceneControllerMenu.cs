using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerMenu : MonoBehaviour
	{
		[SerializeField]
		private GameObject _quitButton;

		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private AudioClip _mouseOverClip;
		[SerializeField]
		private AudioClip _selectClip;

		private void Awake()
		{
			// TODO: Hide the quit button if WebGL
		}

		public void ChangeScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
		
		public void Quit()
		{
			Application.Quit();
		}

		public void MouseOverButton()
		{
			this._audioSource.PlayOneShot(this._mouseOverClip);
		}

		public void MouseSelect()
		{
			this._audioSource.clip = this._selectClip;
			this._audioSource.Play();
		}
	}
}