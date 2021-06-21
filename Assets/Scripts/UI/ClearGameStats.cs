using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
public class ClearGameStats : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {
        TabConfirmManager.NewConfirmTab(
            "Do you <u>really</u> want to clear all the game stats ?<br><br>This action <u>cannot</u> be undone.",
            () => GameStatsSystem.ClearStats(),
            null);
    }

}