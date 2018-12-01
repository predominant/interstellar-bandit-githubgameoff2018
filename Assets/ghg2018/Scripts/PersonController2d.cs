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

        protected float _lastShot = 0f;
        [SerializeField]
        protected float _shotDelay = 0.2f;

        [SerializeField]
        protected GameObject _bulletPrefab;
        [SerializeField]
        protected float _bulletLifetime = 0.5f;
        [SerializeField]
        protected float _bulletForce = 2f;

        [SerializeField]
        protected Transform _bulletSpawnPositionRight;
        [SerializeField]
        protected Transform _bulletSpawnPositionLeft;

        [SerializeField]
        protected AudioSource _shootAudioSource;

        
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

        protected virtual void TakeDamage()
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

        protected bool CanShoot()
        {
            if (Time.time - this._lastShot > this._shotDelay)
                return true;

            // TODO: Can't shoot while searching cargo?
            // TODO: Can't shoot while leaving train (end of level)
			
            return false;
        }
		
        protected virtual void Shoot()
        {
            if (!this.CanShoot())
                return;

            this._shootAudioSource.Play();
			
            this._lastShot = Time.time;
            var pos = (this._facingRight ? this._bulletSpawnPositionRight : this._bulletSpawnPositionLeft).position;

            var bullet = GameObject.Instantiate(
                this._bulletPrefab,
                pos,
                Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce((this._facingRight ? Vector3.right : Vector3.left) * this._bulletForce);
            GameObject.Destroy(bullet, this._bulletLifetime);
        }

        protected abstract void Die();
    }
}