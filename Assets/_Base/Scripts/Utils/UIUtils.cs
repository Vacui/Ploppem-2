using UnityEngine;
using UnityEngine.UI;

namespace Utils {

    public static class UIUtils {
        public static TextMesh CreateWorldText(string text, Transform parent, Vector3 localPosition, Quaternion localRotation, Color fontColor, int fontSize = 40, TextAnchor textAnchor = TextAnchor.MiddleCenter) {
            if (fontColor == null) fontColor = Color.white;
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.fontSize = fontSize;
            textMesh.color = fontColor;
            textMesh.font = GetDefaultFont();
            textMesh.text = text;
            return textMesh;
        }

        // Get Default Unity Font, used in text objects if no font given
        public static Font GetDefaultFont() {
            return Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        // source: https://forum.unity.com/threads/layoutgroup-does-not-refresh-in-its-current-frame.458446/#post-6712318
        public static void RefreshLayoutGroupsImmediateAndRecursive(GameObject root) {
            LayoutGroup[] layoutGroupsInChildren = root.GetComponentsInChildren<LayoutGroup>(true);

            foreach (LayoutGroup layoutGroup in layoutGroupsInChildren) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
            }

            LayoutGroup parentLayoutGroup = root.GetComponent<LayoutGroup>();

            if (parentLayoutGroup != null) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayoutGroup.GetComponent<RectTransform>());
            }
        }
    }
}