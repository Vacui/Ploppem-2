using UnityEngine;
using UnityEngine.Events;

public class TabConfirmManager : MonoBehaviour {

    public static TabConfirmManager Instance;

    [SerializeField] private TabConfirm tabConfirmPrefab;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public static void NewConfirmTab(string title, UnityAction yesCallback, UnityAction noCallback) {

        if(Instance == null) {
            Debug.LogWarning("No active instace of TabConfirmManager");
            return;
        }

        if(Instance.tabConfirmPrefab == null) {
            Debug.LogWarning("TabConfirmManager has no TabConfirm prefab");
            return;
        }

        TabConfirm tabConfirm = GameObject.Instantiate(Instance.tabConfirmPrefab).GetComponent<TabConfirm>();
        tabConfirm.SetUp(title, yesCallback, noCallback);

    }

}