using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(TopDownShipControl))]
	[RequireComponent(typeof(Health))]
	public class Starship : MonoBehaviour
	{
		[SerializeField]
		private GameObject ExplosionObject;

		[SerializeField]
		private GameObject ShipModelObject;

		private Health _health;

		[SerializeField]
		private SceneControllerPlanetHunt _sceneController;
		
		private void Awake()
		{
			this._health = this.GetComponent<Health>();
		}

		private void OnCollisionEnter(Collision other)
		{
			var died = false;
			if (other.gameObject.layer == LayerMask.NameToLayer("Phaser"))
			{
				// Remove the phaser shots immediately
				GameObject.Destroy(other.gameObject);
				died = this.TakeDamage();
			}

			if (other.gameObject.layer == LayerMask.NameToLayer("Planet") || died)
			{
				this.Explode();
			}
		}

		private bool TakeDamage()
		{
			this._health.Damage(5);
			return this._health.Amount == 0;
		}

		private void Explode()
		{
			this.ExplosionObject.SetActive(true);
			this.ShipModelObject.SetActive(false);

			this.GetComponent<TopDownShipControl>().enabled = false;
			Debug.Log("EXPLODE HERE!");

			this._sceneController.Fail();
		}
	}
}