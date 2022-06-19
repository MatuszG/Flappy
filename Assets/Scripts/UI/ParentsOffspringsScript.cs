using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentsOffspringsScript : MonoBehaviour {
    private Button button;

    void Awake() {
        button = GetComponent<Button>();
        button.interactable = !PopulationManager.ParentsOffSprings;
    }

    public void Clicked() {
        PopulationManager.ParentsOffSprings = !PopulationManager.ParentsOffSprings;
    }
}
