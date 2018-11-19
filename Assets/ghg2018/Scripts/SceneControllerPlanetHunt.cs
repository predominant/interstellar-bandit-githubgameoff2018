using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ghg2018
{
	public class SceneControllerPlanetHunt : MonoBehaviour
	{
		[SerializeField]
		private Button ScanButton;

		[SerializeField]
		private Button LandButton;

		[SerializeField]
		private GameObject ScanDataPanel;

		private Planet _scanTarget;

		private Planet ScanTarget
		{
			get { return this._scanTarget; }
			set
			{
				this._scanTarget = value;
				this.ScanButton.interactable = value != null;
				
				this.LandButton.interactable = value != null && this._scanTarget.Jobs.Length > 0;

				if (value == null)
				{
					this.ScanDataPanel.SetActive(false);
				}
			}
		}

		private void Awake()
		{
			this.RegisterHandlers();
		}

		private void RegisterHandlers()
		{
			Planet.OnScannerRangeEntered += this.SetScanTarget;
		}

		private void SetScanTarget(Planet p)
		{
			this.ScanTarget = p;
		}

		public void ScanPlanet()
		{
			this.ScanButton.interactable = false;
			this.ScanDataPanel.SetActive(true);

			foreach (var job in this.ScanTarget.Jobs)
				this.ScanDataPanel.GetComponent<ScanPanel>().AddJob(job);
		}

		public void ChangeScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
	}
}