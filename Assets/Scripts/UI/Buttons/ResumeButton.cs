public class ResumeButton : GameButton {

    protected override void Start() {
        GameHandler.OnGameStarted += DisableButton;
        GameHandler.OnGamePaused += EnableButton;
        GameHandler.OnGameResumed += DisableButton;
        GameHandler.OnGameOver += DisableButton;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= DisableButton;
        GameHandler.OnGamePaused -= EnableButton;
        GameHandler.OnGameResumed -= DisableButton;
        GameHandler.OnGameOver -= DisableButton;
        base.OnDestroy();
    }

    protected override void OnClick() {
        if(GameHandler.Instance == null) {
            return;
        }

        GameHandler.Instance.ResumeGame();
        base.OnClick();
    }

}