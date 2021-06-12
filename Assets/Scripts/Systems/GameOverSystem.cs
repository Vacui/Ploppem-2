using System;
using Unity.Entities;

[UpdateAfter(typeof(KillerEnemySystem))]
[UpdateAfter(typeof(LifetimeJobSystem))]
public class GameOverSystem : ComponentSystem {

    private int lifes;

    public event EventHandler OnGameOver;

    protected override void OnCreate() {
        GameHandler.OnGameStarted += OnGameStarted;
        World.GetOrCreateSystem<LifetimeJobSystem>().OnEnemyDead += OnEnemyDead;
    }

    private void OnGameStarted(object sender, EventArgs args) {

        lifes = GameHandler.Instance.Lifes;

    }

    private void OnEnemyDead(object sender, EventArgs args) {

        lifes--;
        UnityEngine.Debug.Log($"An Enemy is dead, lifes remaining: {lifes}");

    }

    protected override void OnUpdate() {
        
        if(lifes > 0) {
            return;
        }

        OnGameOver?.Invoke(this, EventArgs.Empty);

    }

}