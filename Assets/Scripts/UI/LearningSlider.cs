using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LearningSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;

    void Start() {
        text = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
        slider.value = PopulationManager.LearningRate;
        text.text = slider.value.ToString("n2");
        slider.onValueChanged.AddListener( (v) => {
            PopulationManager.LearningRate = v;
            text.text = v.ToString("n2");
        });
    }
}
