using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerCutscene4 : MonoBehaviour
	{
		private float _changeSceneTime = 12.25f;

		private bool _changingScene = false;
		
		private void Update()
		{
			if (Time.timeSinceLevelLoad >= this._changeSceneTime && !this._changingScene)
			{
				this._changingScene = true;
				SceneManager.LoadScene("credits");
			}
		}
	}
}