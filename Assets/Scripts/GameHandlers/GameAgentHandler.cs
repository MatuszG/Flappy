using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    private GameObject[] pipes;
    
    private void saveAgentMaxScore() {
        if(maxScore < score) {
            FileSystem.SaveAgentMaxScore(score);
        }
    }

    private float getMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }

    private float getAgentScore() {
        return newBird.gameObject.GetComponent<AgentBirdHandler>().Score;
    }

    protected void Update() {
        if(alive) {
            score = getAgentScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveAgentMaxScore();
        }
    }
}
