using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class GetSize : MonoBehaviour {

        public enum Size {
            Height,
            Width,
            Height_And_Width
        }
        [SerializeField] private Size size;
        [SerializeField] private RectTransform target;

        private void Update() {
#if (UNITY_EDITOR)
            UpdateSize();
#endif
        }

        public void UpdateSize() {
            RectTransform rectTransform = GetComponent<RectTransform>();

            if (rectTransform == null) {
                return;
            }

            if (target == null) {
                return;
            }

            float width = rectTransform.sizeDelta.x;
            float height = rectTransform.sizeDelta.y;

            switch (size) {
                case Size.Height:
                    height = target.sizeDelta.y;
                    break;
                case Size.Width:
                    width = target.sizeDelta.x;
                    break;
                case Size.Height_And_Width:
                    height = target.sizeDelta.y;
                    width = target.sizeDelta.x;
                    break;
            }

            rectTransform.sizeDelta = new Vector2(width, height);

            LayoutElement layoutElement = GetComponent<LayoutElement>();

            if(layoutElement != null) {
                switch (size) {
                    case Size.Height:
                        layoutElement.minHeight = height;
                        break;
                    case Size.Width:
                        layoutElement.minWidth = width;
                        break;
                    case Size.Height_And_Width:
                        layoutElement.minHeight = height;
                        layoutElement.minWidth = width;
                        break;
                }
            }
        }

    }
}