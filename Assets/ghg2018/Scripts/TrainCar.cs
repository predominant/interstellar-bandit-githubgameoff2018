using UnityEngine;

namespace ghg2018
{
    public class TrainCar : MonoBehaviour
    {
        [SerializeField]
        private TrainCar _leadCar;

        [SerializeField]
        private float _leadCarSpacing = 0.1f;
        
        [SerializeField]
        private TrainRail _trainRail;

        [SerializeField]
        private float Speed = 1f;

        [HideInInspector]
        public float TrackPosition = 0f;

        private void FixedUpdate()
        {
            this.UpdateTrackPosition();
        }
        
        private void Update()
        {
            this.transform.position = this._trainRail.PositionByRatio(this.TrackPosition);
            this.transform.rotation = Quaternion.LookRotation(this._trainRail.TangentByRatio(this.TrackPosition));
        }

        private void UpdateTrackPosition()
        {
            if (this._leadCar == null)
            {
                this.TrackPosition += this.Speed * Time.fixedDeltaTime;
            }
            else
            {
                this.TrackPosition = this._leadCar.TrackPosition - this._leadCarSpacing;
            }

            // TODO: Replace wrap around
            // Wrap around on the current track
            if (this.TrackPosition < 0f)
                this.TrackPosition += 1f;
            if (this.TrackPosition >= 1f)
                this.TrackPosition -= 1f;
        }
    }
}