using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GetHighscore : MonoBehaviour {

    private TMP_Text text;
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        ScoreSystem.OnNewHighscore += UpdateText;
        UpdateText(ScoreSystem.Highscore);
    }

    private void OnDisable() {
        ScoreSystem.OnNewHighscore -= UpdateText;
    }

    private void UpdateText(int highscore) {
        if (text == null) {
            return;
        }

        text.text = string.Format("{0}{1}{2}", prefix, highscore, suffix);
    }

}