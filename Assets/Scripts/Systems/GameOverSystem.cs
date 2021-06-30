using Unity.Entities;
using UnityEngine.Events;

[UpdateAfter(typeof(KillerEnemySystem))]
[UpdateAfter(typeof(LifetimeJobSystem))]
public class GameOverSystem : ComponentSystem {

    private int lifes;
    public int Lifes {
        get {
            return lifes;
        }
        private set {
            lifes = value;
            OnLifesChanged?.Invoke(lifes);
        }
    }

    public event UnityAction OnGameOver;
    public event UnityAction<int> OnLifesChanged;

    protected override void OnCreate() {
        GameHandler.OnGameStarted += OnGameStarted;
        World.GetOrCreateSystem<LifetimeJobSystem>().OnEnemyDead += OnEnemyDead;
    }

    private void OnGameStarted() {
        Lifes = GameHandler.Instance.Lifes;
    }

    private void OnEnemyDead() {
        Lifes--;
    }

    protected override void OnUpdate() {
        if(Lifes > 0) {
            return;
        }

        OnGameOver?.Invoke();
    }

}