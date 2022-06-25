using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;
    int currentFps = 0;

    void Start() {
        slider.value = Time.timeScale; 
        text = textMesh.GetComponent<TextMeshProUGUI>();
        text.text = slider.value.ToString("n2");
        slider.onValueChanged.AddListener( (v) => {
            Time.timeScale = v; 
        });
    }

    void Update() {
        slider.value = Time.timeScale;
        text.text = slider.value.ToString("n2");
        currentFps = (int) (1 / Time.fixedDeltaTime);
        if(currentFps < 59) {
            slider.interactable = false;
            return;
        }
        else slider.interactable = true;
        if(Time.timeScale < 30f) Time.fixedDeltaTime = 0.0167f;
        if(Time.timeScale >= 30f) Time.fixedDeltaTime = 0.01f;
        if(Time.timeScale >= 30 && currentFps > 99 || currentFps >= 59) {
            if(Time.timeScale < 50 && PopulationManager.AutomaticAcceleration) Time.timeScale += 0.05f;
        }
    }
}
