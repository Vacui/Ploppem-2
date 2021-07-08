using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SkinElement : MonoBehaviour {
    
    private Skin skin;
    private SkinManager skinManager;
    private int index;

    private Toggle toggle;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Image icon;

    public void Initialize(SkinManager skinManager, int index, Skin skin, ToggleGroup toggleGroup, bool isUnlocked, bool isSelected) {

        this.skinManager = skinManager;
        this.index = index;

        if (skin == null) {
            return;
        }

        this.skin = skin;

        UpdateVisuals();

        InitializeToggle(toggleGroup, isUnlocked, isSelected);
    }

    private void UpdateVisuals() {
        if (skin == null) {
            Debug.LogWarning("Skin is null");
            return;
        }

        if (valueText != null) {
            valueText.text = skin.UnlockValue.ToString();
        } else {
            Debug.LogWarning("Text for value is null");
        }

        if (icon != null) {
            icon.sprite = skin.Icon;
        } else {
            Debug.LogWarning("Icon is null");
        }

        name = skin.Title;
        if (titleText != null) {
            titleText.text = skin.Title.ToString();
        } else {
            Debug.LogWarning("Text for title is null");
        }
    }

    private void InitializeToggle(ToggleGroup toggleGroup, bool isUnlocked, bool isSelected) {
        toggle = GetComponent<Toggle>();

        if (toggle == null) {
            Debug.LogWarning("Toggle is null");
            return;
        }

        toggle.onValueChanged.AddListener(Select);

        toggle.group = toggleGroup;

        toggle.interactable = isUnlocked;
        toggle.isOn = isUnlocked ? isSelected : false;
    }

    private void Select(bool value) {
        if (!value) {
            return;
        }

        skinManager.Select(index);
    }

}