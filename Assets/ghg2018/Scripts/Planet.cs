using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(SphereCollider))]
	public class Planet : MonoBehaviour
	{
		public PlanetJob[] Jobs;
		
		public delegate void ScannerRangeEntered(Planet p);
		public static ScannerRangeEntered OnScannerRangeEntered;
		
		private void OnTriggerEnter(Collider other)
		{
			if (OnScannerRangeEntered != null)
				OnScannerRangeEntered(this);
		}

		private void OnTriggerExit(Collider other)
		{
			if (OnScannerRangeEntered != null)
				OnScannerRangeEntered(null);
		}
	}
}