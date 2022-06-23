using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;
    int currentFps;

    void Start() {
        slider.value = Time.timeScale; 
        text = textMesh.GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString("n2");
        slider.onValueChanged.AddListener( (v) => {
            Time.timeScale = v; 
        });
    }

    void Update() {
        if(Time.timeScale < 1) return;
        // else if(Time.timeScale >= 30f) {
        //     Time.fixedDeltaTime = 0.0167f;
        // }
        // else if(Time.timeScale < 30f) {
        //     Time.fixedDeltaTime = 0.0167f;
        // }
        slider.value = Time.timeScale;
        text.text = slider.value.ToString("n2");
        currentFps = (int) (1 / Time.fixedDeltaTime);
        if(currentFps < 58) {
            slider.interactable = false;
            return;
        }
        else slider.interactable = true;
        if(slider.value != Time.timeScale) {
            slider.value = Time.timeScale;
            text.text = slider.value.ToString("n2");
        }
    }
}
