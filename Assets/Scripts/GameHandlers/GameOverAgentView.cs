using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverAgentView : MonoBehaviour {
    [SerializeField] private GameObject agentMaxScoreText;
    
    private void OnEnable() {
        Debug.Log("AgentScore");
        float score = FileSystem.GetAgentMaxScore();
        agentMaxScoreText.gameObject.GetComponent<TextMeshProUGUI>().text = "Max score: " + score.ToString("0");
    }

    public void Restart() {
        Debug.Log("AgentScene");
        SceneManager.LoadScene("AgentScene");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
