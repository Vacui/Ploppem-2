using System;
using UnityEngine;

public class Testing : MonoBehaviour {

    [EditorButton(nameof(ResumeGame), "Resume", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(PauseGame), "Pause", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StartGame), "Play", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StopGame), "Stop", ButtonActivityType.OnPlayMode)]
    public bool temp;

    public static event EventHandler OnGameStarted;
    public static event EventHandler OnGamePaused;
    public static event EventHandler OnGameResumed;
    public static event EventHandler OnGameOver;

    private void StartGame() {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    private void PauseGame() {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    private void ResumeGame() {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    private void StopGame() {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

}