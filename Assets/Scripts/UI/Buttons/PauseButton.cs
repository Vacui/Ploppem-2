public class PauseButton : GameButton {

    protected override void Start() {
        GameHandler.OnGameStarted += EnableButton;
        GameHandler.OnGamePaused += DisableButton;
        GameHandler.OnGameResumed += EnableButton;
        GameHandler.OnGameOver += DisableButton;
        base.Start();
    }

    protected override void OnDestroy() {
        GameHandler.OnGameStarted -= EnableButton;
        GameHandler.OnGamePaused -= DisableButton;
        GameHandler.OnGameResumed -= EnableButton;
        GameHandler.OnGameOver -= DisableButton;
        base.OnDestroy();
    }

    protected override void OnClick() {
        if (GameHandler.Instance == null) {
            return;
        }

        GameHandler.Instance.PauseGame();
        base.OnClick();
    }

}