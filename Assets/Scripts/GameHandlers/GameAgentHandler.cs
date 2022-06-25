using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameAgentHandler : GameHandler {
    [SerializeField] protected GameObject toggleObj;
    protected Toggle toggle;
    protected int lastSavedMaxScore;
	protected const int offsetToSave = 1000;


    private void OnEnable() {
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = PopulationManager.AutomaticAcceleration;
    }

    private void Start() {
        alive = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.0167f;
        maxScore = getMaxScore();
        lastSavedMaxScore = maxScore;
        Instantiate(pipeHandler);
        newBird = Instantiate(birdHandler);
        newBird.gameObject.GetComponent<AgentBirdHandler>().NotificationManager = notificationManager;
        newBird.gameObject.GetComponent<AgentBirdHandler>().getAgentPolicy();
        newBird.gameObject.GetComponent<AgentBirdHandler>().setOn(true);
    }

    private void FixedUpdate() {
        checkKeybordInput();
        if(this.alive) {
            score = getAgentScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("n0");
            saveAgentMaxScore();
        }
    }

    private void Update() {
        //fps = (int) (1 / Time.unscaledDeltaTime);
        checkKeybordInput();
        if (Input.GetKeyDown(KeyCode.Escape) && !alive) {
            GameObject.Find("PanelAgent").GetComponent<GameOverView>().BackToMenu();
        }
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !alive) {
            GameObject.Find("PanelAgent").GetComponent<GameOverView>().Restart();
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
        if(maxScore < score) {
            maxScore = score;
            PopulationManager.MaxPopulationScore = score;
            if(maxScore < offsetToSave || lastSavedMaxScore + offsetToSave == maxScore) {
                lastSavedMaxScore = maxScore;
                NeuralNetwork network = newBird.gameObject.GetComponent<AgentBirdHandler>().Network;
                FileSystem.SaveAgentMaxScore(score);
                /* saving testing agent policy is not required */
                // FileSystem.SaveAgentPolicy(network.getGenome());
            }  
        }
    }   

    private int getMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }

    private int getAgentScore() {
        return newBird.gameObject.GetComponent<AgentBirdHandler>().Score;
    }
}
