using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

namespace ghg2018
{
    public class CargoContainer : MonoBehaviour
    {
        [SerializeField]
        private int _value = 100;
        [SerializeField]
        private bool _looted = false;

        public bool CanLoot()
        {
            return !this._looted && this._value > 0;
        }
        
        public int Loot()
        {
            if (this._looted)
                return 0;
            
            this._looted = true;
            return this._value;
        }
    }
}