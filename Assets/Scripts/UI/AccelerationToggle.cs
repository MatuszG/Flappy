using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccelerationToggle : MonoBehaviour {
    [SerializeField] Toggle toggle;
    
    void Start() {
        toggle.onValueChanged.AddListener( (v) => {
            toggle.isOn = v;
            NetworkManager.AutomaticAcceleration = v;
        });
    }
}
