using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;

    void Start() {
        slider.value = Time.timeScale; 
        text = textMesh.GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString("n3");
        slider.onValueChanged.AddListener( (v) => {
            Time.timeScale = v; 
        });
    }

    void Update() {
        slider.value = Time.timeScale;
        text.text = slider.value.ToString("n3");
        if(slider.value != Time.timeScale) {
            slider.value = Time.timeScale;
            text.text = slider.value.ToString("n3");
        }
    }
}
