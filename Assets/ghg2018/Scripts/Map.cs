using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	public class Map : MonoBehaviour
	{
		[SerializeField]
		private float PositionScale = 0.1f;

		[SerializeField]
		private float ObjectScale = 2f;

		[SerializeField]
		private GameObject PlayerPositionObject;

		[SerializeField]
		private GameObject EnemyShipMarkerPrefab;

		[SerializeField]
		private Transform EnemyMarkerParent;

		private GameObject Player;
		private EnemyShip[] Enemies;
		
		private void Awake()
		{
			this.Player = GameObject.FindWithTag("Player");
			
			var planets = GameObject.FindObjectsOfType<Planet>();
			foreach (var p in planets)
			{
				var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				obj.transform.parent = this.transform;
				var pos = p.transform.position * this.PositionScale;
				pos.y = this.transform.position.y;
				obj.transform.position = pos;
				obj.transform.localScale = p.transform.localScale / 100f * this.ObjectScale;
				if (obj.transform.localScale.magnitude < 1f)
					obj.transform.localScale = Vector3.one;
			}

			this.Enemies = GameObject.FindObjectsOfType<EnemyShip>();
			foreach (var e in this.Enemies)
			{
				var o = GameObject.Instantiate(
					this.EnemyShipMarkerPrefab,
					this.transform.position,
					Quaternion.identity,
					this.EnemyMarkerParent);
				var pos = e.transform.position * this.PositionScale;
				pos.y = this.transform.position.y;
				o.transform.rotation = e.transform.rotation;
			}
		}
		
		private void Update()
		{
			if (this.Player == null)
				return;

			var pos = this.Player.transform.position * this.PositionScale;
			pos.y = this.transform.position.y;
			this.PlayerPositionObject.transform.position = pos;
			this.PlayerPositionObject.transform.rotation = this.Player.transform.rotation;

			for (var i = 0; i < this.Enemies.Length; i++)
			{
				var enemypos = this.Enemies[i].transform.position * this.PositionScale;
				enemypos.y = this.transform.position.y;
				var marker = this.EnemyMarkerParent.GetChild(i);
				marker.position = enemypos;
				marker.transform.rotation = this.Enemies[i].transform.rotation;
			}
		}
	}
}