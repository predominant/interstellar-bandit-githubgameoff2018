﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

namespace ghg2018
{
	public class PlayerController2d : PersonController2d
	{
		public bool Controllable = true;
		
		private Animator _animator;

		public delegate void CargoInRange(CargoContainer c);
		public event CargoInRange OnCargoInRange;

		public delegate void CargoOutOfRange();
		public event CargoOutOfRange OnCargoOutOfRange;

		public delegate void ExtractionInRange();
		public event ExtractionInRange OnExtractionInRange;

		public delegate void ExtractionOutOfRange();
		public event ExtractionOutOfRange OnExtractionOutOfRange;

		private SceneControllerTrainRobbing _sceneController;

		[SerializeField]
		private Image _healthBar;
		
		[SerializeField]
		private float _walkSpeed = 1f;

		[SerializeField]
		private float _boundsXMax = 84f;
		[SerializeField]
		private float _boundsXMin = -13.1f;

		protected new void Awake()
		{
			base.Awake();
			this._sceneController = GameObject.FindObjectOfType<SceneControllerTrainRobbing>();
			this._animator = this.GetComponent<Animator>();
		}
		
		private void Update()
		{
			if (!this.Controllable)
				return;

			var horiz = Input.GetAxis("Horizontal");

			var walking = Mathf.Abs(horiz) > 0.001f;
			this._animator.SetBool("Walking", walking);

			// Don't detect stuff in the dead zone.
			if (walking)
			{
				this.FlipPlayer(horiz > 0f);
				this.transform.Translate(new Vector3(this._walkSpeed * Time.deltaTime * horiz, 0f, 0f));
				if (this.transform.position.x > this._boundsXMax)
				{
					var pos = this.transform.position;
					pos.x = this._boundsXMax;
					this.transform.position = pos;
				}

				if (this.transform.position.x < this._boundsXMin)
				{
					var pos = this.transform.position;
					pos.x = this._boundsXMin;
					this.transform.position = pos;
				}
			}

			
			var shooting = Input.GetAxis("Fire1") > 0.001f;
			this._animator.SetBool("Shooting", shooting);
			if (shooting)
			{
				this.Shoot();
			}
		}

		protected new void OnTriggerEnter(Collider c)
		{
			base.OnTriggerEnter(c);
			if (this.IsCrate(c.gameObject) && this.OnCargoInRange != null)
			{
				var container = c.GetComponent<CargoContainer>();
				if (container.CanLoot())
					this.OnCargoInRange(container);
			}

			if (c.gameObject.layer == LayerMask.NameToLayer("Extraction"))
			{
				this.OnExtractionInRange();
			}
		}

		private void OnTriggerExit(Collider c)
		{
			if (this.IsCrate(c.gameObject) && this.OnCargoOutOfRange != null)
				this.OnCargoOutOfRange();

			if (c.gameObject.layer == LayerMask.NameToLayer("Extraction"))
				this.OnExtractionOutOfRange();
		}

		private bool IsCrate(GameObject g)
		{
			return g.layer == LayerMask.NameToLayer("Crate");
		}

		protected override void TakeDamage()
		{
			Debug.Log("Player taking damage");
			base.TakeDamage();
			this.UpdateHealthUI();
		}

		private void UpdateHealthUI()
		{
			var width = (float)this._health / 10f * 100f;
			Debug.Log("Updating UI for health");
			this._healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 10f);
		}
		
		protected override void Die()
		{
			this._animator.SetBool("Dead", true);
			this._sceneController.FailScene();
		}

		protected override void Shoot()
		{
			this._shootAudioSource.pitch = Random.Range(0.6f, 1.0f);
			base.Shoot();
		}
	}
}