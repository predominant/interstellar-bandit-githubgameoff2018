using UnityEngine;
using UnityEngine.UI;

namespace ghg2018
{
	public class Health : MonoBehaviour
	{
		private int _amount = 100;

		[SerializeField]
		private Image HealthBar;

		[SerializeField]
		private int MaxHealth = 100;

		private int _minHealthSize = 0;
		private int _maxHealthSize = 0;
		private int _barHeight = 0;

		public int Amount
		{
			get { return this._amount; }
			private set
			{
				this._amount = Mathf.Clamp(value, 0, this.MaxHealth);

				if (this.HealthBar != null)
					this.UpdateDisplay();
			}
		}

		private void Awake()
		{
			if (this.HealthBar != null)
			{
				this._maxHealthSize = Mathf.CeilToInt(this.HealthBar.rectTransform.rect.width);
				this._barHeight = Mathf.CeilToInt(this.HealthBar.rectTransform.rect.height);
			}
		}

		public void Damage(int amount)
		{
			this.Amount -= amount;
		}

		public void Heal(int amount)
		{
			this.Amount += amount;
		}

		private void UpdateDisplay()
		{
			//var rect = this.HealthBar.rectTransform.rect;
			var width =
				Mathf.CeilToInt((float) this.Amount / (float) this.MaxHealth * (float) this._maxHealthSize);
			this.HealthBar.rectTransform.sizeDelta = new Vector2(width, this._barHeight);
		}
	}
}