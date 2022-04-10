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
    private List<GameObject> newBirdsList;
    private AgentBirdHandler[] newBirdsHandler;
    private List<AgentBirdHandler> newBirdsHandlerList;
    private TextMeshProUGUI textMeshScore, textMeshEvolution, textMeshMaxScore, textMeshAlive;
    private bool[] notAlive;
    private float[] scores;
    private float currentMaxScore;
    private int numberOfAgents;
    private int aliveNumber, evolutionNumber;
    private GameObject newBird;
    private AgentBirdHandler newBirdHandler;
    private int cratedBirds;

    private void OnEnable() {
        cratedBirds = 0;
        Time.timeScale = 1f;
    }

    private void Awake() {
        newBirdsList = new List<GameObject>();
        newBirdsHandlerList = new List<AgentBirdHandler>();
        Instantiate(pipeHandler);
        numberOfAgents = NetworkManager.NetworksN;
        aliveNumber = numberOfAgents;
        maxScore = getAgentMaxScore();
        scores = new float[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        evolutionNumber = NetworkManager.EvolutionNumber;
        if(evolutionNumber == 0) NetworkManager.create();
        else NetworkManager.mutate();
        // newBirds = new GameObject[numberOfAgents];
        // newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        // for(int i = 0; i < numberOfAgents; i++) createAgent(i);
        // newBirds = newBirdsList.ToArray();
        // newBirdsHandler = newBirdsHandlerList.ToArray();
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString();
        // FileSystem.printPath();
        // C:/Users/mateo/AppData/LocalLow/DefaultCompany/Flappy/
    }

    private void Start() {
        for(int i = 0; i < numberOfAgents; i++) createAgent(i);
        newBirds = newBirdsList.ToArray();
        newBirdsHandler = newBirdsHandlerList.ToArray();
    }

    private void Update() {
        // if(cratedBirds < numberOfAgents) {
        //     createAgent(cratedBirds++);
        // }
        // else {
        //     if(Time.timeScale == 0.01f) Time.timeScale = 1f;
        //     currentMaxScore = 0;
        //     for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        //     for(int i = 0; i < numberOfAgents; i++) update(i);
        //     textMeshScore.text = currentMaxScore.ToString("0");
        //     textMeshAlive.text = "Alive: " + aliveNumber.ToString();
        // }
        currentMaxScore = 0;
        for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        for(int i = 0; i < numberOfAgents; i++) update(i);
        textMeshScore.text = currentMaxScore.ToString("0");
        textMeshAlive.text = "Alive: " + aliveNumber.ToString();

        // MovePipe[] pipes = PipesController.getPipes();
        // if(pipes[0].transform.position.x < 0.6) Debug.Log(pipes[0].transform.position);
        // else if(pipes[0].transform.position.x > 3) Debug.Log(pipes[0].transform.position);
        // for(int i = 0; i < pipes.Length; i++) {
        //     Debug.Log(pipes[i].transform.position);
        // }
        // newBird = Instantiate(birdHandler);
        // newBirdHandler = newBird.GetComponent<AgentBirdHandler>();
        // newBirdHandler.Id = 999;
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

    // private void createAgent(int i) {
    //     newBird = Instantiate(birdHandler);
    //     newBirds[i] = newBird.gameObject;
    //     newBirdHandler = newBird.GetComponent<AgentBirdHandler>();
    //     newBirdHandler.Network = NetworkManager.Networks[i];
    //     newBirdHandler.Id = i;
    //     newBirdsHandler[i] = newBirdHandler;
    // }

    private void createAgent(int i) {
        newBird = Instantiate(birdHandler);
        newBirdsList.Add(newBird.gameObject);
        newBirdHandler = newBird.GetComponent<AgentBirdHandler>();
        newBirdHandler.Network = NetworkManager.Networks[i];
        newBirdHandler.Id = i;
        newBirdsHandlerList.Add(newBirdHandler);
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
