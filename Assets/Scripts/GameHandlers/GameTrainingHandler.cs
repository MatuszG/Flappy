using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTrainingHandler : GameAgentHandler {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject evolutionText;
    [SerializeField] private GameObject maxScoreText;
    [SerializeField] private GameObject aliveText;
    private MainMenu menu;
    private GameObject[] newBirds;
    private AgentBirdHandler[] newBirdsHandler;
    private TextMeshProUGUI textMeshScore, textMeshEvolution, textMeshMaxScore, textMeshAlive;
    private bool[] notAlive;
    private float[] scores;
    private float currentMaxScore;
    private const int numberOfAgents = 200;
    private int aliveNumber, evolutionNumber;

    private void OnEnable() {
        Time.timeScale = 1f;
    }

    private void Awake() {
        Instantiate(pipeHandler);
        aliveNumber = numberOfAgents;
        maxScore = getAgentMaxScore();
        scores = new float[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        newBirds = new GameObject[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        NetworkManager.NetworksN = numberOfAgents;
        evolutionNumber = NetworkManager.EvolutionNumber;
        if(evolutionNumber == 0) NetworkManager.create();
        else NetworkManager.mutate();
        newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        for(int i = 0; i < numberOfAgents; i++) createAgent(i);
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString();
    }

    private void Update() {
        currentMaxScore = 0;
        for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        for(int i = 0; i < numberOfAgents; i++) update(i);
        textMeshScore.text = currentMaxScore.ToString("0");
        textMeshAlive.text = "Alive: " + aliveNumber.ToString();
    }

    private void update(int i) {
        if(!notAlive[i]) {
            scores[i] = getAgentScore(i);
            saveAgentMaxScore(i);
        }
    }

    private void checkAlive(int i) {
        if(!notAlive[i] && !newBirdsHandler[i].Alive) {
            setDeadAgent(i);
        }
    }

    private void setDeadAgent(int i) {
        notAlive[i] = true;
        aliveNumber--;
        Destroy(newBirds[i].gameObject);
        newBirdsHandler[i].updatedNetworkTime();
        newBirdsHandler[i].updateNetworkScore(scores[i]);
        if(aliveNumber == 0) {
            menu.TrainAgain();
        }
    }

    private void createAgent(int i) {
        newBirds[i] = Instantiate(birdHandler);
        newBirdsHandler[i] = newBirds[i].GetComponent<AgentBirdHandler>();
        newBirdsHandler[i].Network = NetworkManager.Networks[i];
        newBirdsHandler[i].Id = i;
    }

    private float getAgentScore(int i) {
        return newBirdsHandler[i].Score;
    }

    private void saveAgentMaxScore(int i) {
        if(maxScore < scores[i]) {
            maxScore = scores[i];
            FileSystem.SaveAgentMaxScore(scores[i]);
        }
        if(currentMaxScore < scores[i]) {
            currentMaxScore = scores[i];
        }
    }

    private float getAgentMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }
}
