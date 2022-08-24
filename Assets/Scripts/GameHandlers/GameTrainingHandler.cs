using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameTrainingHandler : GameAgentHandler {
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject evolutionText;
    [SerializeField] private GameObject maxScoreText;
    [SerializeField] private GameObject aliveText;
    [SerializeField] private bool testing;
    private MainMenu menu;
    private GameObject[] newBirds;
    private AgentBirdHandler[] newBirdsHandler;
    private TextMeshProUGUI textMeshScore, textMeshEvolution, textMeshMaxScore, textMeshAlive;
    private bool[] notAlive;
    private int[] scores;
    private int currentMaxScore, numberOfAgents, aliveNumber, evolutionNumber;
    private List<float> bestPolicy;
    private AgentBirdHandler newBirdHandler;
    private GameObject pipes;
    private MovePipe[] bestPipes;
    private int currentFps;

    private void OnEnable() {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.0167f;
    }

    private void Awake() {
        pipes = Instantiate(pipeHandler);
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = PopulationManager.AutomaticAcceleration;
        numberOfAgents = PopulationManager.NetworksN;
        newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        // FileSystem.printPath();
        // C:/Users/mateo/AppData/LocalLow/DefaultCompany/Flappy/
        // C:/Users/garba/AppData/LocalLow/DefaultCompany/Flappy/
    }

    private void Start() {
        PopulationManager.Testing = testing;
        bestPolicy = new List<float>();
        aliveNumber = numberOfAgents;
        maxScore = getAgentMaxScore();
        lastSavedMaxScore = maxScore;
        scores = new int[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        evolutionNumber = PopulationManager.EvolutionNumber;
        if(evolutionNumber == 0) {
            PopulationManager.create();
            for(int i = 0; i < numberOfAgents; i++) createAgent(i);
            for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].setOn(true);
        }
        else {
            PopulationManager.evolve();
			for(int i = 0; i < numberOfAgents; i++) newBirdsHandler[i].restart(i);
        }
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString("0");
    }

    protected new void checkKeybordInput() {
        if (Input.GetKeyDown(KeyCode.Delete)) {
            FileSystem.ResetAgentScore();
            FileSystem.ResetAgentPolicy();
        }
    }

    private void Update() { return; }

    private void Restart() {
        Time.timeScale = 1f;
        pipes.GetComponent<PipeHandler>().Restart();
        PopulationManager.EvolutionNumber++;
        Start();
    }

    private void FixedUpdate() {
        checkKeybordInput();
        setBestPipes();
        currentMaxScore = 0;
        for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        for(int i = 0; i < numberOfAgents; i++) update(i);
        textMeshScore.text = currentMaxScore.ToString("n0");
        textMeshAlive.text = "Alive: " + aliveNumber.ToString();
        if(Test.ScoreUpperBound < currentMaxScore && testing) {
            testing = false;
            Time.timeScale = 0;
            PopulationManager.MaxPopulationScore = currentMaxScore;
            PopulationManager.EvolutionNumber++;
            PopulationManager.evolve();
            menu.ContinueTesting();
        }
    }

    private void setBestPipes() {
        if(PipesController.getPipes() != bestPipes) {
            bestPipes = PipesController.getPipes();
        }
        if(bestPipes == null) return;
        for(int i = 0; i < numberOfAgents; i++) {
            newBirdsHandler[i].setBestPipes(bestPipes);
        }
    }

    private void update(int i) {
        if(!notAlive[i]) {
            scores[i] = getAgentScore(i);
            newBirdsHandler[i].updateNetworkScore(scores[i]);
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
        newBirdHandler.Id = i;
        newBirdHandler.Network = PopulationManager.Networks[i];
        newBirdsHandler[i] = newBirdHandler;
    }

    private int getAgentScore(int i) {
        return newBirdsHandler[i].Score;
    }

    private void saveAgentMaxScore(int i) {
        if(maxScore < scores[i]) {
            maxScore = scores[i];
            PopulationManager.MaxPopulationScore = maxScore;
            if(maxScore < offsetToSave || lastSavedMaxScore + offsetToSave == maxScore) {
                lastSavedMaxScore = maxScore;
                if(bestPolicy != PopulationManager.Networks[i].getGenome()) {
                    newBirdsHandler[i].saveAgentPolicy();
                }
                FileSystem.SaveAgentMaxScore(scores[i]);
            }
        }
        if(currentMaxScore < scores[i]) {
            currentMaxScore = scores[i];
        }
    }

    private int getAgentMaxScore() {
        return FileSystem.GetAgentMaxScore();
    }
}
