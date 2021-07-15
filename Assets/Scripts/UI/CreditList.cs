using UnityEngine;

public class CreditList : MonoBehaviour {

    [SerializeField] private CreditPanel creditPanelPrefab;
    private Credit[] creditsArray;

    private void OnEnable() {
        // Clear credits
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        creditsArray = Resources.LoadAll<Credit>("UI/Credits");

        GenerateCredits();
    }

    private void GenerateCredits() {
        if(creditPanelPrefab == null) {
            Debug.LogWarning("No credit panel prefab");
            return;
        }

        foreach(Credit credit in creditsArray) {
            CreditPanel newCreditPanel = Instantiate(creditPanelPrefab, transform).GetComponent<CreditPanel>();
            newCreditPanel.SetUp(credit);
        }
    }


}