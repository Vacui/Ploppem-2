using UnityEngine;
using Utils;

namespace UI {
    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class Tab : UIElement {
        [Header("Tab Settings")]
        [SerializeField] private TabGroup group;
        public TabGroup Group => group;
        [SerializeField] private bool useCustomName = false;
        [SerializeField, EnableIf("useCustomName", true)] private string customName = "";

        [Header("On Active")]
        [SerializeField] private bool showChildrens = true;
        [SerializeField, ReorderableList] private GameObject[] objToShowOnActive;
        [SerializeField, ReorderableList] private GameObject[] objToHideOnActive;

        protected override void Start() {
            base.Start();
            if (group != null) {
                group.Subscribe(GetName(), this);
            }
        }

        protected override void OnActive() {
            base.OnActive();

            if(group != null) {
                group.ShowTab(GetName());
            }

            UpdateChildrens();
            UtilsClass.SetObjectsActive(objToShowOnActive, IsActive);
            UtilsClass.SetObjectsActive(objToHideOnActive, !IsActive);
        }

        protected override void OnInactive() {
            base.OnInactive();
            UpdateChildrens();
            UtilsClass.SetObjectsActive(objToShowOnActive, IsActive);
            UtilsClass.SetObjectsActive(objToHideOnActive, !IsActive);
        }

        private void UpdateChildrens() {
            if (!showChildrens) {
                return;
            }

            foreach (Transform child in transform) {
                child.gameObject.SetActive(IsActive);
            }
        }

        public string GetName() {
            return (useCustomName ? customName : name).ToLower();
        }
    }
}