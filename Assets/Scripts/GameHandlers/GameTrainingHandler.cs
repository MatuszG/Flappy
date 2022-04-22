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
    private float currentMaxScore, elapsedTime;
    private int numberOfAgents, aliveNumber, evolutionNumber;
    private AgentBirdHandler newBirdHandler;
    private GameObject pipes;

    private void OnEnable() {
        automaticAcceleration = NetworkManager.AutomaticAcceleration;
    }

    private void Awake() {
        pipes = Instantiate(pipeHandler);
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = NetworkManager.AutomaticAcceleration;
        numberOfAgents = NetworkManager.NetworksN;
        newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        // FileSystem.printPath();
        // C:/Users/mateo/AppData/LocalLow/DefaultCompany/Flappy/
    }

    private void Start() {
        elapsedTime = 0;
        Time.timeScale = 1f;
        aliveNumber = numberOfAgents;
        maxScore = getAgentMaxScore();
        scores = new float[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        evolutionNumber = NetworkManager.EvolutionNumber;
        if(evolutionNumber == 0) {
            NetworkManager.create();
            for(int i = 0; i < numberOfAgents; i++) createAgent(i);
            for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].setOn(true);
        }
        else {
            NetworkManager.mutate();
            // for(int i = 0; i < numberOfAgents; i++) createAgent(i);
            for(int i = 0; i < numberOfAgents; i++) {
                Destroy(newBirdsHandler[i].gameObject);
                createAgent(i);
            }
            for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].setOn(true);
            // for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].restart(i);
            // for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].setOn(true);
        }
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString();
    }

    private void Restart() {
        Time.timeScale = 0.1f;
        pipes.GetComponent<PipeHandler>().Restart();
        NetworkManager.EvolutionNumber++;
        Start();
    }

    private void FixedUpdate() {
        autoSpeed();
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
        newBirdsHandler[i].setActive(false);
        newBirdsHandler[i].updatedNetworkTime();
        newBirdsHandler[i].updateNetworkScore(scores[i]);
        if(aliveNumber == 0) {
            Restart();
        }
    }

    private void createAgent(int i) {
        newBird = Instantiate(birdHandler);
        newBirdHandler = newBird.GetComponent<AgentBirdHandler>();
        newBirdHandler.Network = NetworkManager.Networks[i];
        newBirdHandler.Id = i;
        newBirdsHandler[i] = newBirdHandler;
    }

    private float getAgentScore(int i) {
        return newBirdsHandler[i].Score;
    }

    private void saveAgentMaxScore(int i) {
        if(maxScore < scores[i]) {
            maxScore = scores[i];
            FileSystem.SaveAgentMaxScore(scores[i]);
            newBirdsHandler[i].saveAgentPolicy();
        }
        if(currentMaxScore < scores[i]) {
            currentMaxScore = scores[i];
        }
    }

    private float getAgentMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }
}
