using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    [SerializeField] protected GameObject toggleObj;
    protected bool automaticAcceleration;
    protected Toggle toggle;

    public void changeAcceleration() {
        automaticAcceleration = !automaticAcceleration;
        NetworkManager.AutomaticAcceleration = automaticAcceleration;
    }

    private void OnEnable() {
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = NetworkManager.AutomaticAcceleration;
        automaticAcceleration = NetworkManager.AutomaticAcceleration;
    }

    protected void autoSpeed(int alive = 0) {
        toggle.isOn = NetworkManager.AutomaticAcceleration;
        automaticAcceleration = NetworkManager.AutomaticAcceleration;
        // Soft start
        // if(Time.timeScale < 1f) {
        //     Time.timeScale += 0.001f;
        // }
        // else 
        if(automaticAcceleration) {
            if(Time.timeScale < 1.5f && alive < 500) {
                Time.timeScale += 0.003f;
            }
            else if(Time.timeScale < 3f && alive < 200) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 5f && alive < 100) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 7f && alive < 50) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 10f && alive < 30) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 15f && alive < 10) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 20f && alive < 5) { 
                Time.timeScale += 0.01f;
            }
        }
    }

    private void Start() {
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
            maxScore = score;
            FileSystem.SaveAgentMaxScore(score);
            NeuralNetwork network = newBird.gameObject.GetComponent<AgentBirdHandler>().Network;
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
            autoSpeed(0);
            score = getAgentScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveAgentMaxScore();
        }
    }
}
