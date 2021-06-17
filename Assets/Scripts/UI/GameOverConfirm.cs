using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
public class GameOverConfirm : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {
        TabConfirmManager.NewConfirmTab(
            "Do you <u>really</u> want to stop the game ?",
            () => GameHandler.Instance.StopGame(),
            null);
    }

}