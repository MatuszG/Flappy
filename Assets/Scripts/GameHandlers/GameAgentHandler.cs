using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    private void OnEnable() {
        maxScore = getMaxScore();
        Time.timeScale = 1f;
        alive = true;
        newBird = Instantiate(birdHandler);
        Instantiate(pipeHandler);
    }

    private void saveMaxScore() {
        if(maxScore < score) {
            FileSystem.SaveAgentMaxScore(score);
        }
    }

    private float getMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }

    private float getScore() {
        return newBird.gameObject.GetComponent<AgentBirdHandler>().getScore();
    }

    protected void Update() {
        if(alive) {
            score = getScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveMaxScore();
        }
    }
}
