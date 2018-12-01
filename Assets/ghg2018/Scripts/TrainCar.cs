using System;
using BansheeGz.BGSpline.Components;
using UnityEngine;

namespace ghg2018
{
    public class TrainCar : MonoBehaviour
    {
        private static SceneControllerTrainChase SceneController;
        
        /// <summary>
        /// Start at the very end of the first segment, so the whole train fits on the track
        /// </summary>
        [SerializeField]
        private float _startPosition = 0.9f;

        [SerializeField]
        private TrainCar _leadCar;

        [SerializeField]
        private float _leadCarSpacing = 0.1f;
        
        [SerializeField]
        private TrainRail _trainRail;

        [SerializeField]
        private float Speed = 1f;

        //[HideInInspector]
        public float TrackPosition = 0f;

        [SerializeField]
        private float RequiredScanTime = 5f;

        public bool HasCargo = false;

        private bool _scanned = false;

        public bool Scanned
        {
            get { return this._scanned; }
            set
            {
                if (!this._scanned)
                {
                    this._scanned = value;
                    this.SetMaterialTint();
                    SceneController.ScannedCars++;
                }
            }
        }
        
        private float _scanStartTime = 0f;
        private float _scanDuration = 0f;
        
        private bool _scanning = false;
        public bool Scanning
        {
            get { return this._scanning; }
            set
            {
                this._scanning = value;
                if (this._scanning)
                    this._scanStartTime = Time.time;
                else
                    this._scanDuration = Time.time - this._scanStartTime;

                if (this._scanDuration > this.RequiredScanTime)
                    this.Scanned = true;
            }
        }

        [SerializeField]
        private float RequiredAlignTime = 10f;
        private float _alignStartTime = 0f;
        private float _alignDuration = 0f;

        private bool _aligned = false;
        public bool Aligned
        {
            get { return this._aligned; }
            set
            {
                if (!this._aligned)
                {
                    this._aligned = value;
                    SceneController.Aligned = true;
                }
            }
        }

        private bool _aligning = false;
        public bool Aligning
        {
            get { return this._aligning; }
            set
            {
                this._aligning = value;
                if (this._aligning)
                    this._alignStartTime = Time.time;
                else
                    this._alignDuration = Time.time - this._alignStartTime;

                if (this._alignDuration > this.RequiredAlignTime)
                    this.Aligned = true;
            }
        }

        private void Awake()
        {
            SceneController = GameObject.Find("Scene Controller").GetComponent<SceneControllerTrainChase>();
            this.TrackPosition = this._startPosition;
        }

        private void FixedUpdate()
        {
            this.UpdateTrackPosition();

            switch (SceneController.Mode)
            {
                case TrainSceneMode.Scan:
                    this.CheckScanTime();
                    break;
                case TrainSceneMode.Align:
                    this.CheckAlignTime();
                    break;
            }
        }
        
        protected virtual void Update()
        {
            //this.transform.position = this._trainRail.PositionByRatio(this.TrackPosition);
            this.transform.position = this._trainRail.PositionByDistance(this.TrackPosition);
            this.transform.rotation = Quaternion.LookRotation(this._trainRail.TangentByDistance(this.TrackPosition));
        }

        private void UpdateTrackPosition()
        {
            if (this._leadCar == null)
            {
                // I am a train engine, leading the cars
                this.TrackPosition += this.Speed * Time.fixedDeltaTime;
                
                if (this.TrackPosition >= this._trainRail.GetComponent<BGCcMath>().GetDistance())
                {
                    if (this.NextTrackSegment())
                        this.TrackPosition = 0;
                    else
                    {
                        // TODO: End of track
                    }
                }
            }
            else
            {
                // I am a train car, just following.
                this._trainRail = this._leadCar._trainRail;
                this.TrackPosition = this._leadCar.TrackPosition - this._leadCarSpacing;
                
                if (this.TrackPosition < 0f)
                {
                    this.PreviousTrackSegment();
                    this.TrackPosition += this._trainRail.GetComponent<BGCcMath>().GetDistance();
                }
            }
        }

        private bool PreviousTrackSegment()
        {
            if (this._trainRail.PreviousTrack != null)
            {
                this._trainRail = this._trainRail.PreviousTrack.GetComponent<TrainRail>();
                return true;
            }

            return false;
        }
        
        private bool NextTrackSegment()
        {
            if (this._trainRail.NextTrack != null)
            {
                this._trainRail = this._trainRail.NextTrack.GetComponent<TrainRail>();
                return true;
            }

            return false;
        }
        
        private void OnTriggerEnter(Collider c)
        {
            switch (SceneController.Mode)
            {
                // SCANNING MODE
                case TrainSceneMode.Scan:
                    if (this.Scanned)
                        return;
                    if (c.gameObject.layer != LayerMask.NameToLayer("Scanner"))
                        return;
                    this.Scanning = true;
                    break;
                // ALIGNMENT MODE
                case TrainSceneMode.Align:
                    if (c.gameObject.layer != LayerMask.NameToLayer("Scanner") || !this.HasCargo)
                        return;
                    this.Aligning = true;
                    Debug.Log("Starting alignment");
                    break;
            }
        }

        private void OnTriggerExit(Collider c)
        {
            switch (SceneController.Mode)
            {
                // SCANNING MODE
                case TrainSceneMode.Scan:
                    if (c.gameObject.layer != LayerMask.NameToLayer("Scanner"))
                        return;
                    this.Scanning = false;
                    break;
                // ALIGNMENT MODE
                case TrainSceneMode.Align:
                    if (c.gameObject.layer != LayerMask.NameToLayer("Scanner") || !this.HasCargo)
                        return;
                    this.Aligning = false;
                    Debug.Log("Stopping alignment");
                    break;
            }
        }

        private void SetMaterialTint()
        {
            var material = this.transform.GetChild(0).GetComponent<Renderer>().material;
            if (this.Scanned)
                material.color = this.HasCargo ? Color.green : Color.red;
            else
                material.color = Color.white;
        }

        private void CheckScanTime()
        {
            if (!this._scanning)
                return;

            if (Time.time > this._scanStartTime + this.RequiredScanTime)
            {
                this.Scanning = false;
                this.Scanned = true;
            }
        }

        private void CheckAlignTime()
        {
            if (!this._aligning)
                return;


            Debug.Log("Align Time: " + (Time.time - this._alignStartTime));

            if (Time.time > this._alignStartTime + this.RequiredAlignTime)
            {
                Debug.Log("Align Time Complete");
                this.Aligning = false;
                this.Aligned = true;
            }
        }
    }
}