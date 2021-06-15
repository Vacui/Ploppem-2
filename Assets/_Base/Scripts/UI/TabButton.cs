using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
    public class TabButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TabGroup group;
        [SerializeField] private bool goBackButton = false;
        [SerializeField, ShowIf("goBackButton", false)] private Tab tabToShow;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(group == null) {
                return;
            }

            if (goBackButton) {
                group.GoBack();
                return;
            }

            if(tabToShow == null) {
                return;
            }

            group.ShowTab(tabToShow);
        }
    }
}