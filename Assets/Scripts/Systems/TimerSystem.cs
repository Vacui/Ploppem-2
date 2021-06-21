using Unity.Entities;
using UnityEngine.Events;

public class TimerSystem : ComponentSystem {

    private static float gameTime;
    public static float GameTime {
        get { return gameTime; }
        private set {
            gameTime = value;
            OnTimerChanged?.Invoke(gameTime);
        }
    }

    public static event UnityAction<float> OnTimerChanged;

    protected override void OnCreate() {
        DOTS_GameHandler.Instance.OnGameStarted += Reset;
    }

    protected override void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= Reset;
    }

    private void Reset() {
        GameTime = 0f;
    }

    protected override void OnUpdate() {

        if (!HasSingleton<GameState>() ||
            GetSingleton<GameState>().Value != GameState.State.Playing) {
            return;
        }

        GameTime += Time.DeltaTime;

    }

}