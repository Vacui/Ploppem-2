using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    public static GameHandler Instance { get; private set; }

    [EditorButton(nameof(ResumeGame), "Resume", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(PauseGame), "Pause", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StopGame), "Stop", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StartGame), "Play", ButtonActivityType.OnPlayMode)]
    public bool temp;

    public static event EventHandler OnGameStarted;
    public static event EventHandler OnGamePaused;
    public static event EventHandler OnGameResumed;
    public static event EventHandler OnGameOver;

    private void Awake() {
        
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

    }

    public void StartGame() {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public void PauseGame() {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame() {
        OnGameResumed?.Invoke(this, EventArgs.Empty);
    }

    public void StopGame() {
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

}