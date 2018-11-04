using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class ScanPanel : MonoBehaviour
	{
		[SerializeField]
		private Transform JobList;

		[SerializeField]
		private GameObject JobListItemPrefab;
		
		private void OnEnable()
		{
			this.Reset();
		}

		private void OnDisable()
		{
			
		}

		private void Reset()
		{
			foreach (Transform item in this.JobList)
			{
				GameObject.Destroy(item.gameObject);
			}
		}

		public void AddJob(PlanetJob job)
		{
			var jobObject = GameObject.Instantiate(
				this.JobListItemPrefab,
				this.JobList,
				false);
			jobObject.GetComponent<PlanetJobListItem>().SetJob(job);
		}
	}
}