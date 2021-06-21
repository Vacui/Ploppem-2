using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {

    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class TabButton : MonoBehaviour, IPointerClickHandler {
        [SerializeField] private bool goBackButton = false;
        [SerializeField, ShowIf("goBackButton", true)] private TabGroup group;
        [SerializeField, ShowIf("goBackButton", false)] private Tab tabToShow;

        public void OnPointerClick(PointerEventData eventData) {

            if (goBackButton) {
                if (group == null) {
                    return;
                }

                group.GoBack();
                return;
            } else {
                if (tabToShow == null) {
                    return;
                }

                if (tabToShow.Group == null) {
                    return;
                }

                tabToShow.Group.ShowTab(tabToShow);
            }

        }
    }
}