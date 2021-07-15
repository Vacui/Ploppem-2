using Doozy.Engine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour {

    private Skin[] skinArray;
    private const string SELECTED_SKIN_INDEX_KEY = "skin";
    private int UnlockedSkin {
        get { return Mathf.Clamp(PlayerPrefs.GetInt(SELECTED_SKIN_INDEX_KEY, 0), 0, skinArray.Length - 1); }
        set { PlayerPrefs.SetInt(SELECTED_SKIN_INDEX_KEY, Mathf.Clamp(value, 0, skinArray.Length - 1)); }
    }

    [SerializeField] private string unlockSkinPopupName;

    private static List<Skin> gamesessionSkinUnlocked;
    public static bool NewSkins => gamesessionSkinUnlocked != null && gamesessionSkinUnlocked.Count > 0;
    private int oldHighscore;

    [Header("Customization UI")]
    [SerializeField] private Transform skinElementsParent;
    [SerializeField] private SkinElement startSkinElementPrefab;
    [SerializeField] private SkinElement middleSkinElementPrefab;
    [SerializeField] private SkinElement endSkinElementPrefab;
    [SerializeField] private GameObject separatorPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    private void Awake() {
        skinArray = Resources.LoadAll<Skin>("UI/Skins");
        skinArray = skinArray.OrderBy(s => s.UnlockValue).ToArray();
        UnlockedSkin = Mathf.Clamp(UnlockedSkin, 0, skinArray.Where(s => s.UnlockValue <= ScoreSystem.Highscore).Count() - 1);
    }

    private void OnEnable() {
        ScoreSystem.OnNewHighscore += NewHighscore;
        GameHandler.OnGameStarted += Reset;
    }

    private void OnDisable() {
        ScoreSystem.OnNewHighscore -= NewHighscore;
        GameHandler.OnGameStarted -= Reset;
    }

    public void UpdateSkinElements() {
        // clear skin elements
        foreach (Transform child in skinElementsParent) {
            Destroy(child.gameObject);
        }

        // generate new ones
        SkinElement newSkinElement;

        toggleGroup.allowSwitchOff = true;

        for (int i = 0; i < skinArray.Length; i++) {

            if (i == 0) {
                newSkinElement = Instantiate(startSkinElementPrefab, skinElementsParent);
            } else {
                Instantiate(separatorPrefab, skinElementsParent);
                if (i == skinArray.Length - 1) {
                    newSkinElement = Instantiate(endSkinElementPrefab, skinElementsParent);
                } else {
                    newSkinElement = Instantiate(middleSkinElementPrefab, skinElementsParent);
                }
            }

            newSkinElement.Initialize(this, i, skinArray[i], toggleGroup, IsUnlocked(skinArray[i].UnlockValue), IsSelected(i));
        }

        toggleGroup.allowSwitchOff = false;
    }

    
    private bool IsSelected(int index) {
        return UnlockedSkin == index;
    }
    private bool IsUnlocked(int unlockValue) {
        return ScoreSystem.Highscore >= unlockValue;
    }

    public void Select(int index) {
        // I trust the toggle group to stop the player from selecting locked skins

        UnlockedSkin = index;

        if (EnemySpawnerData.Instance == null) {
            Debug.LogWarning("Enemy Spawner Data is null");
            return;
        }

        EnemySpawnerData.Instance.SetMesh(skinArray[index].Mesh);
    }

    private void NewHighscore(int newHighscore) {

        if(newHighscore <= 0) {
            return;
        }

        if(newHighscore < oldHighscore) {
            return;
        }

        for(int i = 0; i < skinArray.Length; i++) {

            Skin skin = skinArray[i];

            if(skin.UnlockValue <= 0) {
                continue;
            }

            if (skin.UnlockValue <= oldHighscore) {
                // already unlocked
                continue;
            }

            if(skin.UnlockValue > newHighscore) {
                // can't unlock
                break;
            }

            // to unlock

            if (gamesessionSkinUnlocked == null) {
                gamesessionSkinUnlocked = new List<Skin>();
            }

            if (gamesessionSkinUnlocked.Contains(skin)) {
                continue;
            }
            gamesessionSkinUnlocked.Add(skin);

            UIPopup popup = UIPopup.GetPopup(unlockSkinPopupName);

            if (popup == null) {
                Debug.LogWarning("Popup is null");
                return;
            }

            popup.Data.SetLabelsTexts(skin.Title);
            popup.Data.SetImagesSprites(skin.Icon);

            UIPopupManager.ShowPopup(popup, true, false);
        }

        oldHighscore = newHighscore;

    }

    private void Reset() {
        gamesessionSkinUnlocked = new List<Skin>();
        oldHighscore = ScoreSystem.Highscore;
        Select(PlayerPrefs.GetInt(SELECTED_SKIN_INDEX_KEY, 0));
    }

}