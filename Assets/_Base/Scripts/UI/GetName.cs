using TMPro;
using UnityEngine;

namespace UI {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class GetName : MonoBehaviour {
        [SerializeField] private Transform parent;

        private void Awake() {
#if (UNITY_EDITOR)
            if (parent == null) {
                parent = transform.parent;
                GetParentNameToString();
            }
#endif
            GetParentNameToString();
        }

        private void Update() {
#if (UNITY_EDITOR)
            GetParentNameToString();
#endif
        }

        private void GetParentNameToString() {
            if (parent == null) {
                return;
            }

            GetComponent<TextMeshProUGUI>().text = parent.name;
        }
    }
}