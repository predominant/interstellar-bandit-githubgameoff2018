using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

namespace ghg2018
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlayerController2d : PersonController2d
	{
		public bool Controllable = true;
		
		private SpriteRenderer _renderer;
		private Animator _animator;

		public delegate void CargoInRange(CargoContainer c);
		public event CargoInRange OnCargoInRange;

		public delegate void CargoOutOfRange();
		public event CargoOutOfRange OnCargoOutOfRange;

		private SceneControllerTrainRobbing _sceneController;
		
		[SerializeField]
		private float _walkSpeed = 1f;

		private float _lastShot = 0f;
		[SerializeField]
		private float _shotDelay = 0.2f;

		private bool _facingRight = true;

		[SerializeField]
		private float _boundsXMax = 84f;
		[SerializeField]
		private float _boundsXMin = -13.1f;

		[SerializeField]
		private GameObject _bulletPrefab;
		[SerializeField]
		private float _bulletLifetime = 0.5f;
		[SerializeField]
		private float _bulletForce = 2f;

		[SerializeField]
		private Transform _bulletSpawnPositionRight;
		[SerializeField]
		private Transform _bulletSpawnPositionLeft;

		[SerializeField]
		private Image _healthBar;
		
		private void Awake()
		{
			this._sceneController = GameObject.FindObjectOfType<SceneControllerTrainRobbing>();
			this._animator = this.GetComponent<Animator>();
			this._renderer = this.GetComponent<SpriteRenderer>();
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

		private void FlipPlayer(bool facingRight)
		{
			if (facingRight && this._renderer.flipX)
			{
				this._renderer.flipX = false;
				this._facingRight = true;
			}

			if (!facingRight && !this._renderer.flipX)
			{
				this._renderer.flipX = true;
				this._facingRight = false;
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
		}

		private void OnTriggerExit(Collider c)
		{
			if (this.IsCrate(c.gameObject) && this.OnCargoOutOfRange != null)
				this.OnCargoOutOfRange();
		}

		private bool IsCrate(GameObject g)
		{
			return g.layer == LayerMask.NameToLayer("Crate");
		}

		private bool CanShoot()
		{
			if (Time.time - this._lastShot > this._shotDelay)
				return true;

			// TODO: Can't shoot while searching cargo?
			// TODO: Can't shoot while leaving train (end of level)
			
			return false;
		}
		
		private void Shoot()
		{
			if (!this.CanShoot())
				return;

			this._lastShot = Time.time;
			var pos = (this._facingRight ? this._bulletSpawnPositionRight : this._bulletSpawnPositionLeft).position;

			var bullet = GameObject.Instantiate(
				this._bulletPrefab,
				pos,
				Quaternion.identity);
			bullet.GetComponent<Rigidbody>().AddForce((this._facingRight ? Vector3.right : Vector3.left) * this._bulletForce);
			GameObject.Destroy(bullet, this._bulletLifetime);
		}

		protected new void TakeDamage()
		{
			base.TakeDamage();
			this.UpdateHealthUI();
		}

		private void UpdateHealthUI()
		{
			var width = (float)this._health / 10f * 100f;
			this._healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 10f);
		}
		
		protected override void Die()
		{
			this._animator.SetBool("Dead", true);
			this._sceneController.FailScene();
		}
	}
}