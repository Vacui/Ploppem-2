using Doozy.Engine;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class StopGame : MonoBehaviour, IPointerClickHandler {

    [Header("Popup Settings")]
    [SerializeField] private string popupName = "YesNo";
    [SerializeField] private string message;

    public void OnPointerClick(PointerEventData eventData) {
        ShowPopup();
    }

    public void ShowPopup() {
        if (GameHandler.Instance == null) {
            return;
        }

        UIPopup popup = UIPopup.GetPopup(popupName);

        if (popup == null) {
            return;
        }

        popup.Data.SetLabelsTexts(message);

        popup.Data.SetButtonsCallbacks(
            () => GameEventMessage.SendEvent("StopGame"),
            null
            );

        popup.Show();
    }

}