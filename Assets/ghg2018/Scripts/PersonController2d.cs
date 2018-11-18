using UnityEngine;

namespace ghg2018
{
    public abstract class PersonController2d : MonoBehaviour
    {
        [SerializeField]
        protected int _health = 3;

        protected bool Dead
        {
            get { return this._health <= 0; }
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

        protected abstract void Die();
    }
}