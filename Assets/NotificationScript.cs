using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationScript : MonoBehaviour {
    [SerializeField] private GameObject notification;
    private float maxTimeNotification; 
    private float timeNotification;

    public void ShowNotification(string info) {
        timeNotification = 0;
        notification.SetActive(true);
        notification.GetComponent<TextMeshProUGUI>().text = info;
        notification.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0f, 2f, true);
    }

    void Start() {
        maxTimeNotification = 2f;
        timeNotification = maxTimeNotification;
    }

    void Update() {
        if(timeNotification >= maxTimeNotification) {
            notification.SetActive(false);
        }
        timeNotification += Time.deltaTime;
    }
}
