using System;
using UnityEngine;

public class NewHighscore : MonoBehaviour {

    [SerializeField] private GameObject objectToShow;

    private void Start() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsSystem.OnNewHighscore += EnableObject;
        DisableObject(null, EventArgs.Empty);
    }

    private void OnDestroy() {
        GameHandler.OnGameStarted += DisableObject;
        GameStatsSystem.OnNewHighscore -= EnableObject;
    }

    private void EnableObject(object sender, EventArgs args) {
        if (objectToShow == null) {
            return;
        }

        objectToShow.SetActive(true);
    }
    private void DisableObject(object sender, EventArgs args) {
        if (objectToShow == null) {
            return;
        }

        objectToShow.SetActive(false);
    }

}