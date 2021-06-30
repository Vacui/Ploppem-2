using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour {

    public static GameHandler Instance { get; private set; }

    public static event UnityAction OnGameStarted;
    public static event UnityAction OnGamePaused;
    public static event UnityAction OnGameResumed;
    public static event UnityAction OnGameOver;

    [EditorButton(nameof(ResumeGame), "Resume", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(PauseGame), "Pause", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StartGame), "Play", ButtonActivityType.OnPlayMode)]
    [EditorButton(nameof(StopGame), "Stop", ButtonActivityType.OnPlayMode)]
    [SerializeField] private int lifes;
    public int Lifes => lifes;

    private void Awake() {
        
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;

    }

    private void Start() {
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<GameOverSystem>().OnGameOver += StopGame;

        GameStatsManager.Initialize();
    }

    public void StartGame() {
        OnGameStarted?.Invoke();
    }

    public void PauseGame() {
        OnGamePaused?.Invoke();
        System.GC.Collect();
    }

    public void ResumeGame() {
        OnGameResumed?.Invoke();
    }

    public void StopGame() {
        OnGameOver?.Invoke();
        System.GC.Collect();
    }

}