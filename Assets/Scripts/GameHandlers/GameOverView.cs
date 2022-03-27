using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverView : MonoBehaviour {
    [SerializeField] private GameObject maxScoreText;
    
    private void OnEnable() {
        float score = FileSystem.GetMaxScore();
        maxScoreText.gameObject.GetComponent<TextMeshProUGUI>().text = "Max score: " + score.ToString("0");
    }

    public void Restart() {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
