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
	}
}