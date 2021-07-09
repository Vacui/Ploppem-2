using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;

[UpdateAfter(typeof(KillerEnemySystem))]
public class ScoreSystem : ComponentSystem {

    private static int highscore = -1;
    public static int Highscore {
        get {
            if(highscore < 0) {
                highscore = PlayerPrefs.GetInt(KEY_HIGHSCORE, 0);
            }
            return highscore;
        }
        private set {
            if (value <= highscore || value <= 0) {
                IsHighscore = false;
                return;
            }

            highscore = value;
            IsHighscore = true;
            PlayerPrefs.SetInt(KEY_HIGHSCORE, highscore);
            OnNewHighscore?.Invoke(highscore);
        }
    }
    public static bool IsHighscore { get; private set; }
    private const string KEY_HIGHSCORE = "highscore";
    public static event UnityAction<int> OnNewHighscore;

    private static int score;
    public static int Score {
        get { return score; }
        private set {
            score = value;
            OnScoreChanged?.Invoke(score);
            Highscore = value;
        }
    }
    public static event UnityAction<int> OnScoreChanged;

    protected override void OnCreate() {
        if(DOTS_GameHandler.Instance == null) {
            Debug.LogError("DOTS_GameHandler instance is null");
        }

        DOTS_GameHandler.Instance.OnGameStarted += Reset;

        World.GetOrCreateSystem<KillerEnemySystem>().OnKilledEnemy += KilledEnemy;
        World.GetOrCreateSystem<KillerEnemySystem>().OnMissedEnemy += MissedEnemy;
    }

    protected override void OnDestroy() {
        if (DOTS_GameHandler.Instance == null) {
            UnityEngine.Debug.LogError("DOTS_GameHandler instance is null");
        }

        DOTS_GameHandler.Instance.OnGameStarted -= Reset;
    }

    private void Reset() {
        Score = 0;
        IsHighscore = false;
    }

    private void KilledEnemy(int enemies) {
        Score += enemies;
    }

    private void MissedEnemy() {
        
    }

    protected override void OnUpdate() { }

}