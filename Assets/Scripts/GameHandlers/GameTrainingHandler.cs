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
    private BirdPool pool;
    private float elapsedTime;

    private void OnEnable() {
        // cratedBirds = 0;
        Time.timeScale = 1f;
    }

    private void Awake() {
        // newBirdsList = new List<GameObject>();
        // newBirdsHandlerList = new List<AgentBirdHandler>();
        // newBirds = new GameObject[numberOfAgents];
        // Instantiate(pipeHandler);
        Instantiate(pipeHandler);
        numberOfAgents = NetworkManager.NetworksN;
        aliveNumber = numberOfAgents;
        newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        maxScore = getAgentMaxScore();
        scores = new float[numberOfAgents];
        notAlive = new bool[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        evolutionNumber = NetworkManager.EvolutionNumber;
        if(evolutionNumber == 0) NetworkManager.create();
        else NetworkManager.mutate();
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString();
        elapsedTime = 0;
        // FileSystem.printPath();
        // C:/Users/mateo/AppData/LocalLow/DefaultCompany/Flappy/
    }

    private void Start() {
        // PoolsHandler.restart();
        // PoolsHandler.play();
        // pool = Test.pool;
        // Debug.Log(pool.pooledObjects.Count);
        for(int i = 0; i < numberOfAgents; i++) createAgent(i);
        // newBirdsHandler = newBirdsHandlerList.ToArray();
        for(int i = 0; i < numberOfAgents; i++) {
            // newBirdsHandler[i].setActive(true);
            newBirdsHandler[i].setOn(true);
        }
    }

    private void FixedUpdate() {
        // if(Time.timeScale < 1.5f) {
        //     Time.timeScale += 0.003f;
        // }
        // else if(Time.timeScale < 10f) {
        //     Time.timeScale += 0.01f;
        // }
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
            menu.TrainAgain();
        }
    }

    private void createAgent(int i) {
        newBird = Instantiate(birdHandler);
        newBirdHandler = newBird.GetComponent<AgentBirdHandler>();
        newBirdHandler.Network = NetworkManager.Networks[i];
        newBirdHandler.Id = i;
        newBirdsHandler[i] = newBirdHandler;
    }

    // private void createAgent(int i) {
    //     GameObject bird = pool.GetPooledObject(i);
    //     newBirdHandler = bird.GetComponent<AgentBirdHandler>();
    //     newBirdHandler.Network = NetworkManager.Networks[i];
    //     newBirdHandler.Id = i;
    //     newBirdsHandlerList.Add(newBirdHandler);
    // }

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
