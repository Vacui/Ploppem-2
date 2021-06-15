using System;
using Unity.Entities;

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
            OnLifesChanged?.Invoke(this, new LifesEventArgs { remainingLifes = lifes });
        }
    }

    public event EventHandler OnGameOver;
    public event EventHandler<LifesEventArgs> OnLifesChanged;
    public class LifesEventArgs : EventArgs {
        public int remainingLifes;
    }

    protected override void OnCreate() {
        GameHandler.OnGameStarted += OnGameStarted;
        World.GetOrCreateSystem<LifetimeJobSystem>().OnEnemyDead += OnEnemyDead;
    }

    private void OnGameStarted(object sender, EventArgs args) {

        Lifes = GameHandler.Instance.Lifes;

    }

    private void OnEnemyDead(object sender, EventArgs args) {

        Lifes--;
        UnityEngine.Debug.Log($"An Enemy is dead, lifes remaining: {Lifes}");

    }

    protected override void OnUpdate() {
        
        if(Lifes > 0) {
            return;
        }

        OnGameOver?.Invoke(this, EventArgs.Empty);

    }

}