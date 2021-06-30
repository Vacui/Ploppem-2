using Unity.Entities;
using UnityEngine.Events;

[UpdateAfter(typeof(KillerEnemySystem))]
public class ScoreSystem : ComponentSystem {

    private static int score;
    public static int Score {
        get { return score; }
        private set {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }

    public static event UnityAction<int> OnScoreChanged;

    protected override void OnCreate() {
        DOTS_GameHandler.Instance.OnGameStarted += Reset;
        DOTS_GameHandler.Instance.OnGameOver += Reset;

        World.GetOrCreateSystem<KillerEnemySystem>().OnKilledEnemy += KilledEnemy;
        World.GetOrCreateSystem<KillerEnemySystem>().OnMissedEnemy += MissedEnemy;
    }

    protected override void OnDestroy() {
        DOTS_GameHandler.Instance.OnGameStarted -= Reset;
        DOTS_GameHandler.Instance.OnGameOver -= Reset;
    }

    private void Reset() {
        Score = 0;
    }

    private void KilledEnemy(int enemies) {
        Score += enemies;
    }

    private void MissedEnemy() {
        
    }

    protected override void OnUpdate() { }

}