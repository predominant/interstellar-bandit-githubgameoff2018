using UnityEngine;

namespace com.ggames4u {

    /// <summary>
    /// This script has an example of how to get access to the warp shpere from the root game object.
    /// </summary>
    [HelpURL("http://blog.ggames4u.com/wp-content/uploads/2018/09/WarpEffect_Manual_Version-1.4.0-1.pdf")]
    public class WarpEffect : MonoBehaviour {
        [SerializeField]
        private Material warpMaterial;

        /// <summary>
        /// This script has an example of how to get access to the warp shpere from the root game object.
        /// </summary>
        private void Awake() {
            GameObject warpSphere = this.transform.Find("WarpSphereContainer").Find("WarpSphere").gameObject;
            this.warpMaterial = warpSphere.GetComponent<Renderer>().sharedMaterial;
            if (this.warpMaterial != null) {
                // Do somthing
            }
        }
    }
}
