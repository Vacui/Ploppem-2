using System;
using Unity.Entities;

public class TimerSystem : ComponentSystem {

    private float gameTime;
    public float GameTime {
        get { return gameTime; }
        private set {
            gameTime = value;
            OnTimerChanged?.Invoke(this, new TimerChangedEventArgs { time = gameTime });
        }
    }

    public event EventHandler<TimerChangedEventArgs> OnTimerChanged;
    public class TimerChangedEventArgs : EventArgs {
        public float time;
    }

    protected override void OnCreate() {
        DOTS_GameHandler.Instance.OnGameStarted += Reset;
    }

    protected override void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= Reset;
    }

    private void Reset(object sender, EventArgs args) {
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