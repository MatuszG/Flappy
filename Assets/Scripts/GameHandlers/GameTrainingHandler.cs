using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTrainingHandler : MonoBehaviour {
    [SerializeField] private GameObject birdHandler;
    [SerializeField] private GameObject pipeHandler;
    [SerializeField] private GameObject scoreText;
    private GameObject[] newBirds;
    private GameObject newBird;
    private bool[] notAlive;
    private float[] score;
    private float maxScore, currentMaxScore;
    private const int numberOfAgents = 10;

    private void Start() {
        notAlive = new bool[numberOfAgents];
        score = new float[numberOfAgents];
        newBirds = new GameObject[numberOfAgents];
        Debug.Log(newBirds.Length);
        Time.timeScale = 1f;
        maxScore = getAgentMaxScore();
        Instantiate(pipeHandler);
        for(int i = 0; i < numberOfAgents; i++) {
            createAgent(i);
            newBirds[i].GetComponent<BirdHandler>().Id = i;
            Debug.Log(newBirds[i].GetComponent<BirdHandler>().Id);
        }
    }

    public void setDead(int i) {
        notAlive[i] = true;
        Debug.Log("setDead");
        if(lastAlive()) {
            Debug.Log("RESTARTING");
            // restart
        }
    }

    private void Update() {
        currentMaxScore = 0;
        for(int i = 0; i < numberOfAgents; i++) update(i);
        scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = currentMaxScore.ToString("0");
    }

    private void update(int i) {
        if(!notAlive[i]) {
            score[i] = getAgentScore(i);
            saveAgentMaxScore(i);
        }
        else {
            Destroy(newBirds[i].gameObject);
        }
    }

    private bool lastAlive() {
        int alive = 0;
        for(int i = 0; i < notAlive.Length; i++) {
            if(!notAlive[i]) alive++;
        }
        if(alive == 1) return true;
        return false;
    }

    private void createAgent(int i) {
        newBird = Instantiate(birdHandler);
        Debug.Log(newBird);
        newBirds[i] = newBird.gameObject;
    }

    private float getAgentScore(int i) {
        return newBirds[i].gameObject.GetComponent<AgentBirdHandler>().Score;
    }

    private void saveAgentMaxScore(int i) {
        if(maxScore < score[i]) {
            maxScore = score[i];
            FileSystem.SaveAgentMaxScore(score[i]);
        }
        if(currentMaxScore < score[i]) {
            currentMaxScore = score[i];
        }
    }

    private float getAgentMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }
}
