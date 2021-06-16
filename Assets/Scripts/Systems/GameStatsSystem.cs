using System;
using Unity.Entities;
using UnityEngine;

public class GameStatsSystem : ComponentSystem {

    private const string HIGHSCORE_KEY = "highscore";
    private float highscore = -1f;
    private float Highscore {
        get { return highscore; }
        set {
            if (highscore > value) {
                return;
            }

            highscore = value;
            OnNewHighscore?.Invoke(value);
        }
    }
    public static event Action<float> OnNewHighscore;

    protected override void OnCreate() {
        World.GetOrCreateSystem<TimerSystem>().OnTimerChanged += UpdateHighscore;
        DOTS_GameHandler.Instance.OnGameOver += SaveStats;

        GetStats();

        base.OnCreate();
    }

    protected override void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameOver -= SaveStats;

        SaveStats();
    }

    protected override void OnUpdate() { }

    private void UpdateHighscore(object sender, TimerSystem.TimerChangedEventArgs args) {
        Highscore = args.time;
    }

    private void GetStats() {
        Highscore = PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f);
    }
    public static float GetHighscore() { return PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f); }

    private void SaveStats() {
        if (Highscore > PlayerPrefs.GetFloat(HIGHSCORE_KEY, 0f)) {
            PlayerPrefs.SetFloat(HIGHSCORE_KEY, Highscore);
        }
    }
    private void SaveStats(object sender, EventArgs args) {
        SaveStats();
    }

}