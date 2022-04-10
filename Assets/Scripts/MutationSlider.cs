using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MutationSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;

    void Start() {
        text = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
        slider.value = NetworkManager.MutateRatio * 100;
        text.text = (NetworkManager.MutateRatio * 100).ToString("0") + "%";
        slider.onValueChanged.AddListener( (v) => {
            NetworkManager.MutateRatio = v/100f;
            text.text = v.ToString("0") + "%";
        });
    }
}
