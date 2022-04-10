using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {
    [SerializeField] private Slider slider;

    void Start() {
        // Time.timeScale = 1f;
        slider.value = Time.timeScale; 
        slider.onValueChanged.AddListener( (v) => {
            Time.timeScale = v; 
        });
    }
}
