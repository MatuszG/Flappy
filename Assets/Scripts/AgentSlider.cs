using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentSlider : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject textMesh;
    private TextMeshProUGUI text;

    void Start() {
        text = textMesh.gameObject.GetComponent<TextMeshProUGUI>();
        slider.value = NetworkManager.NetworksN;
        slider.onValueChanged.AddListener( (v) => {
            NetworkManager.NetworksN = (int)v;
            text.text = v.ToString();
        });
    }
}
