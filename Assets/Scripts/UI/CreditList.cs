using System.Collections.Generic;
using UnityEngine;

public class CreditList : MonoBehaviour {

    [SerializeField] private CreditPanel creditPanelPrefab;
    [SerializeField] [ReorderableList] private List<Credit> credits;

    private void OnEnable() {
        // Clear credits
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        GenerateCredits();
    }

    private void GenerateCredits() {
        if(creditPanelPrefab == null) {
            Debug.LogWarning("No credit panel prefab");
            return;
        }

        foreach(Credit credit in credits) {
            CreditPanel newCreditPanel = Instantiate(creditPanelPrefab, transform).GetComponent<CreditPanel>();
            newCreditPanel.SetUp(credit);
        }
    }


}