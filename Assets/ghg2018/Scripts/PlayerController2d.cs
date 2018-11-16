using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ghg2018
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class PlayerController2d : MonoBehaviour
	{
		public bool Controllable = true;
		
		private SpriteRenderer _renderer;
		private Animator _animator;

		public delegate void CargoInRange(CargoContainer c);
		public event CargoInRange OnCargoInRange;

		public delegate void CargoOutOfRange();
		public event CargoOutOfRange OnCargoOutOfRange;
		
		[SerializeField]
		private float _walkSpeed = 1f;
		
		private void Awake()
		{
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
			}
		}

		private void FlipPlayer(bool facingRight)
		{
			if (facingRight && this._renderer.flipX)
				this._renderer.flipX = false;

			if (!facingRight && !this._renderer.flipX)
				this._renderer.flipX = true;
		}

		private void OnTriggerEnter(Collider c)
		{
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
	}
}