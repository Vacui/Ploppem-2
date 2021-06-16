using UnityEngine;
using TMPro;
using Utils;

[RequireComponent(typeof(TMP_Text))]
public class Highscore : MonoBehaviour {

    private TMP_Text text;

    [SerializeField] private string prefix;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        prefix = prefix.Replace("\\n", "\n");
    }

    private void Start() {
        GameStatsSystem.OnNewHighscore += UpdateText;
        UpdateText(GameStatsSystem.GetHighscore());
    }

    private void OnDestroy() {
        GameStatsSystem.OnNewHighscore -= UpdateText;
    }

    private void UpdateText(float highscore) {
        if (text == null) {
            return;
        }

        string highscoreFormatted = string.Empty;
        if (highscore >= 3600f) {
            highscoreFormatted = UtilsClass.FormatTimeWithHours(highscore);
        } else {
            highscoreFormatted = UtilsClass.FormatTime(highscore);
        }

        text.text = prefix + highscoreFormatted;
    }

}