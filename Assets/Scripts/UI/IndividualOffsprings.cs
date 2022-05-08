using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndividualOffsprings : MonoBehaviour {
    private bool parentOffSprings;
    private Button button;

    void Awake() {
        button = GetComponent<Button>();
        button.interactable = PopulationManager.ParentOffSprings;
    }

    public void Clicked() {
        PopulationManager.ParentOffSprings = !PopulationManager.ParentOffSprings;
    }
}