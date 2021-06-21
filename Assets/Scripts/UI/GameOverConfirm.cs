using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
public class GameOverConfirm : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {
        if(GameHandler.Instance == null) {
            Debug.LogWarning("GameHandler Instance is null");
            return;
        }

        TabConfirmManager.NewConfirmTab(
            "Do you <u>really</u> want to stop the game ?",
            () => GameHandler.Instance.StopGame(),
            null);
    }

}