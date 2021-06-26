using UnityEngine;

namespace UI {
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class DisableOcclusionCulling : MonoBehaviour {

        private void Awake() {
            GetComponent<Camera>().useOcclusionCulling = false;
        }

    }
}