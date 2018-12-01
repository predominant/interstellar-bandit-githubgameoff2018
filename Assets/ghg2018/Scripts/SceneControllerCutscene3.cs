using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerCutscene3 : MonoBehaviour
	{
		[SerializeField]
		private float _nextSceneTime = 11.5f;
		
		private bool _changingScene = false;
		
		private void Update()
		{
			if (Time.timeSinceLevelLoad >= this._nextSceneTime && !this._changingScene)
			{
				this._changingScene = true;
				SceneManager.LoadScene("game-3-train-robbing");
			}
		}
	}
}