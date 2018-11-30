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

		[SerializeField]
		private GameObject _failedPanel;

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

		private void OnDestroy()
		{
			this.UnregisterHandlers();
		}

		private void UnregisterHandlers()
		{
			Planet.OnScannerRangeEntered -= this.SetScanTarget;
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

			if (this.ScanTarget.Jobs == null || this.ScanTarget.Jobs.Length == 0)
				this.ScanDataPanel.GetComponent<ScanPanel>().NoJobs();
		}

		public void ChangeScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}

		public void Fail()
		{
			this._failedPanel.SetActive(true);
		}

		public void Retry()
		{
			this.ChangeScene(SceneManager.GetActiveScene().name);
		}

		public void Quit()
		{
			this.ChangeScene("menu");
		}
	}
}