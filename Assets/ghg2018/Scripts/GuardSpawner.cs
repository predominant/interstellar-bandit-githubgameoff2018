using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class GuardSpawner : MonoBehaviour
	{
		[SerializeField]
		private int _guardCount = 5;

		[SerializeField]
		private GameObject _guardPrefab;

		[SerializeField]
		private Transform _guardParent;

		[SerializeField]
		private float _spawnRange = 3.4f;
		
		private void Awake()
		{
			// TODO: Check to see if I am a "safe" car.
		}

		private void Start()
		{
			for (var i = 0; i < this._guardCount; i++)
			{
				this.SpawnGuard();
			}
		}

		private void SpawnGuard()
		{
			GameObject.Instantiate(
				this._guardPrefab,
				this.RandomGuardPos(),
				Quaternion.identity,
				this._guardParent);
		}

		private Vector3 RandomGuardPos()
		{
			var refPos = this._guardParent.position;
			refPos.x = this.transform.position.x;
			var offset = Random.Range(
				Mathf.Abs(this._spawnRange) * -1f,
				Mathf.Abs(this._spawnRange));
			return refPos + new Vector3(offset, 0f, 0f);
		}
	}
}