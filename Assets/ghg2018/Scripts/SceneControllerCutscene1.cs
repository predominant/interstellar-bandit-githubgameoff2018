using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerCutscene1 : MonoBehaviour
	{
		[SerializeField]
		private CinemachineDollyCart _dollyCart;

		[SerializeField]
		private string _nextScene = "menu";
		
		private bool _changingScene = false;

		private void Update()
		{
			if (this._dollyCart.m_Position >= 1f && !this._changingScene)
			{
				this._changingScene = true;
				this.ChangeScene(this._nextScene);
			}
		}

		private void ChangeScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
	}
}