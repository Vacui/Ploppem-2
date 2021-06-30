using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GetGameStat : MonoBehaviour {

    private TMP_Text text;

    [SerializeField] private GameStatsManager.GameStatSource source;
    [SerializeField] private GameStatsManager.GameStatType gameStat;
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;
    [SerializeField] private string nullValue;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        GameStatsManager.OnUpdateAllStats += UpdateText;
        UpdateText();
    }

    private void OnDisable() {
        GameStatsManager.OnUpdateAllStats -= UpdateText;
    }

    private void UpdateText() {
        if (text == null) {
            return;
        }

        string stat = GameStatsManager.GetStat(source, gameStat);

        if (stat == string.Empty) {
            stat = nullValue;
        }

        text.text = string.Format("{0}{1}{2}", prefix, stat, suffix);
    }

}