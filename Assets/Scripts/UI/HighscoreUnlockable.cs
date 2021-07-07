using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Toggle))]
public class HighscoreUnlockable : MonoBehaviour {

    private Toggle toggle;
    [SerializeField, EditorButton(nameof(UpdateText), "Update Text", ButtonActivityType.Everything)] private TMP_Text textValue;
    [SerializeField] private int unlockValue;
    [SerializeField] private Mesh mesh;

    private void Awake() {
        toggle = GetComponent<Toggle>();

        if(toggle == null) {
            Debug.LogWarning("Toggle is null");
            return;
        }

        toggle.onValueChanged.AddListener(Unlock);

        UpdateText();
    }

    private void UpdateText() {
        if (textValue == null) {
            Debug.LogWarning("Text for value is null");
            return;
        }

        textValue.text = unlockValue.ToString();
    }

    private void OnEnable() {
        if (ScoreSystem.Highscore >= unlockValue) {
            toggle.interactable = true;
            if (IsSelected(unlockValue)) {
                toggle.isOn = true;
                Unlock(true);
            }
        } else {
            toggle.interactable = false;
            toggle.isOn = false;
        }
    }

    private void Unlock(bool value) {
        if(value == true) {
            if (EnemySpawnerData.Instance != null) {
                EnemySpawnerData.Instance.SetMesh(mesh);
            }
            Select(unlockValue);
        }
    }

    private const string SKIN_KEY = "skin";
    private static bool IsSelected(int unlockValue) {
        return PlayerPrefs.GetInt(SKIN_KEY, 0) == unlockValue;
    }
    private static void Select(int unlockValue) {
        PlayerPrefs.SetInt(SKIN_KEY, unlockValue);
    }
}