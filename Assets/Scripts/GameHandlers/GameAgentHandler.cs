using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    private void OnEnable() {
        alive = true;
        Time.timeScale = 1f;
        maxScore = getMaxScore();
        Instantiate(pipeHandler);
        newBird = Instantiate(birdHandler);
        newBird.gameObject.GetComponent<AgentBirdHandler>().getAgentPolicy();
        newBird.gameObject.GetComponent<AgentBirdHandler>().setOn(true);
        // Debug.Log(newBird.gameObject.GetComponent<AgentBirdHandler>().Network);
    }
    
    private void saveAgentMaxScore() {
        if(maxScore < score) {
            NeuralNetwork network = newBird.gameObject.GetComponent<AgentBirdHandler>().Network;
            FileSystem.SaveAgentMaxScore(score);
            FileSystem.SaveAgentPolicy(network.getGenome());
        }
    }   

    private float getMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }

    private float getAgentScore() {
        return newBird.gameObject.GetComponent<AgentBirdHandler>().Score;
    }

    private void FixedUpdate() {
        if(alive) {
            score = getAgentScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveAgentMaxScore();
        }
    }
}
