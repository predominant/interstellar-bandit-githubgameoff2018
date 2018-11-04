using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ghg2018
{
	public class PlanetJobListItem : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI Text;
		
		public void SetJob(PlanetJob job)
		{
			this.Text.text = string.Format(
				this.Text.text,
				job.Name,
				job.Value);
		}
	}
}