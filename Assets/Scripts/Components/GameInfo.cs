using Unity.Entities;

[GenerateAuthoringComponent]
public struct GameInfo : IComponentData {

    public enum GameState {
        WaitingToStart,
        Playing,
        Dead
    }

    public GameState State;
}