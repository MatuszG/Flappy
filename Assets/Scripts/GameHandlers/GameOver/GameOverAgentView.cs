using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverAgentView : MonoBehaviour {
    [SerializeField] private GameObject agentMaxScoreText;
    
    private void OnEnable() {
        float score = FileSystem.GetAgentMaxScore();
        if(agentMaxScoreText) agentMaxScoreText.gameObject.GetComponent<TextMeshProUGUI>().text = "Max score: " + score.ToString("0");
    }

    public void Restart() {
        SceneManager.LoadScene("AgentScene");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
