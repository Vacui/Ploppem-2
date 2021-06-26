using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GetGameStat : MonoBehaviour {

    private TMP_Text text;

    [SerializeField] GameStatsSystem.GameStat gameStat;
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        GameStatsSystem.OnUpdateAllStats += UpdateText;
        UpdateText();
    }

    private void OnDisable() {
        GameStatsSystem.OnUpdateAllStats -= UpdateText;
    }

    private void UpdateText() {
        if (text == null) {
            return;
        }

        text.text = string.Format("{0}{1}{2}", prefix, GameStatsSystem.GetStat(gameStat), suffix);
    }

}