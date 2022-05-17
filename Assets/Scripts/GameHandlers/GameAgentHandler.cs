using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    [SerializeField] protected GameObject toggleObj;
    protected bool automaticAcceleration;
    protected Toggle toggle;
    protected int fps;
    protected float lastMaxScore;

    public void changeAcceleration() {
        automaticAcceleration = !automaticAcceleration;
        PopulationManager.AutomaticAcceleration = automaticAcceleration;
    }

    private void OnEnable() {
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = PopulationManager.AutomaticAcceleration;
        automaticAcceleration = PopulationManager.AutomaticAcceleration;
    }

    private void Start() {
        alive = true;
        Time.timeScale = 1f;
        maxScore = getMaxScore();
        lastMaxScore = maxScore;
        Instantiate(pipeHandler);
        newBird = Instantiate(birdHandler);
        newBird.gameObject.GetComponent<AgentBirdHandler>().NotificationManager = notificationManager;
        newBird.gameObject.GetComponent<AgentBirdHandler>().getAgentPolicy();
        newBird.gameObject.GetComponent<AgentBirdHandler>().setOn(true);
    }

    private void FixedUpdate() {
        checkKeybordInput();
        // Debug.Log(newBird.transform.position);
        if(this.alive) {
            autoSpeed(0);
            score = getAgentScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveAgentMaxScore();
        }
    }

    private void Update() {
        fps = (int) (1 / Time.unscaledDeltaTime);
        checkKeybordInput();
        if (Input.GetKeyDown(KeyCode.Escape) && !alive) {
            GameObject.Find("PanelAgent").GetComponent<GameOverView>().BackToMenu();
        }
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !alive) {
            GameObject.Find("PanelAgent").GetComponent<GameOverView>().Restart();
        }
    }

    protected void autoSpeed(int alive = 0) {
        toggle.isOn = PopulationManager.AutomaticAcceleration;
        automaticAcceleration = PopulationManager.AutomaticAcceleration;
        // Soft start
        // if(Time.timeScale < 1f) {
        //     Time.timeScale += 0.001f;
        // }
        // else 
        if(automaticAcceleration) {
            // if(Time.timeScale < 40 && fps > 120) {
            //     Time.timeScale += 0.01f;
            // }
            if(Time.timeScale < 1f) return;
            else if(Time.timeScale < 1.5f && alive < 500) {
                Time.timeScale += 0.003f;
            }
            else if(Time.timeScale < 3f && alive < 250) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 5f && alive < 150) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 7f && alive < 100) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 10f && alive < 75) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 15f && alive < 50) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 20f && alive < 30) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 30f && alive < 10) { 
                Time.timeScale += 0.01f;
            }
            else if(Time.timeScale < 50f && alive < 5) { 
                Time.timeScale += 0.01f;
                Time.fixedDeltaTime = 0.01f;
            }
            // else if(Time.timeScale < 250f && alive < 1) { 
            //     Time.timeScale += 0.01f;
            // }
        }
    }

    protected void checkKeybordInput() {
        if (Input.GetKeyDown(KeyCode.Delete)) {
            FileSystem.ResetAgentScore();
            notificationManager.GetComponent<NotificationScript>().ShowNotification("Agent score has been deleted!");
            // FileSystem.ResetAgentPolicy();
        }
    }

    private void saveAgentMaxScore() {
        if(maxScore < 10 || lastMaxScore + 10000 == maxScore) {
            lastMaxScore = maxScore;
            Debug.Log("TEST");
        }
        if(maxScore < score) {
            maxScore = score;
            PopulationManager.MaxPopulationScore = (int)score;
            if(maxScore < 10000 || lastMaxScore + 10000 == maxScore) {
                lastMaxScore = maxScore;
                NeuralNetwork network = newBird.gameObject.GetComponent<AgentBirdHandler>().Network;
                FileSystem.SaveAgentMaxScore(score);
                FileSystem.SaveAgentPolicy(network.getGenome());
            }  
        }
    }   

    private float getMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }

    private float getAgentScore() {
        return newBird.gameObject.GetComponent<AgentBirdHandler>().Score;
    }
}
