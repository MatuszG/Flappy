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
    private float[] score;
    private float currentMaxScore;
    private const int numberOfAgents = 200;
    private int aliveNumber, evolutionNumber;
    private int[] topology;
    private NeuralNetwork net;

    private void OnEnable() {
        topology = new int[]{2,3,1};
        net = new NeuralNetwork(topology);
        net.propagate();
        Time.timeScale = 1f;
        Instantiate(pipeHandler);
    }

    private void Awake() {
        aliveNumber = numberOfAgents;
        maxScore = getAgentMaxScore();
        score = new float[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        newBirds = new GameObject[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        evolutionNumber = PipesController.EvolutionNumber;
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
        pipes = PipesController.getPipes();
        for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        for(int i = 0; i < numberOfAgents; i++) update(i);
        textMeshScore.text = currentMaxScore.ToString("0");
        textMeshAlive.text = "Alive: " + aliveNumber.ToString();
    }

    private void update(int i) {
        if(!notAlive[i]) {
            score[i] = getAgentScore(i);
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
        if(aliveNumber == 0) {
            menu.TrainAgain();
        }
    }

    private void createAgent(int i) {
        newBirds[i] = Instantiate(birdHandler);
        newBirdsHandler[i] = newBirds[i].GetComponent<AgentBirdHandler>();
        newBirdsHandler[i].Id = i;
    }

    private float getAgentScore(int i) {
        return newBirdsHandler[i].Score;
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
