using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParentsOffspringsScript : MonoBehaviour {
    private bool parentOffSprings;

    public void Clicked() {
        PopulationManager.ParentOffSprings = !PopulationManager.ParentOffSprings;
    }
}
