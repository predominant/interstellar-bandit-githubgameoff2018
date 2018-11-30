using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ghg2018
{
	public class SceneControllerCredits : MonoBehaviour
	{
		public void ChangeScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
	}
}