using UnityEngine;

namespace ghg2018
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class PersonController2d : MonoBehaviour
    {
        [SerializeField]
        protected int _health = 3;

        protected bool Dead
        {
            get { return this._health <= 0; }
        }

        protected SpriteRenderer _renderer;
        protected bool _facingRight = true;

        protected void Awake()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
        }

        protected void OnTriggerEnter(Collider c)
        {
            if (this.Dead)
                return;
			
            if (c.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                GameObject.Destroy(c.gameObject);
                this.TakeDamage();
            }
        }

        protected void TakeDamage()
        {
            this._health--;
            if (this.Dead)
                this.Die();
        }

        protected void FlipPlayer(bool facingRight)
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

        protected abstract void Die();
    }
}