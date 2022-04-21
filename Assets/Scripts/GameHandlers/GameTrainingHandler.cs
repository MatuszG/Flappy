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
    [SerializeField] private GameObject toggleObj;
    private Toggle toggle;
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
    private bool automaticAcceleration;
    private bool restarting;

    public void changeAcceleration() {
        automaticAcceleration = !automaticAcceleration;
        NetworkManager.AutomaticAcceleration = automaticAcceleration;
    }

    private void OnEnable() {
        automaticAcceleration = NetworkManager.AutomaticAcceleration;
        restarting = false;
    }

    private void Awake() {
        Debug.Log("XD");
        Instantiate(pipeHandler);
        toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = NetworkManager.AutomaticAcceleration;
        numberOfAgents = NetworkManager.NetworksN;
        newBirdsHandler = new AgentBirdHandler[numberOfAgents];
        menu = mainMenu.GetComponent<MainMenu>();
        textMeshScore = scoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshEvolution = evolutionText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshMaxScore = maxScoreText.gameObject.GetComponent<TextMeshProUGUI>();
        textMeshAlive = aliveText.gameObject.GetComponent<TextMeshProUGUI>();
        elapsedTime = 0;
        // FileSystem.printPath();
        // C:/Users/mateo/AppData/LocalLow/DefaultCompany/Flappy/
    }

    private void start() {
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
            for(int i = 0; i < numberOfAgents; i++) {
                newBirdsHandler[i].restart(i);
            }
        }
        textMeshEvolution.text = "Evolution: " + evolutionNumber.ToString();
        textMeshMaxScore.text = "Max score: " + maxScore.ToString();
    }

    private void Start() {
        start();
    }

    private void restart() {
        pipeHandler.GetComponent<PipeHandler>().Restart();
        NetworkManager.EvolutionNumber++;
        // Debug.Log(NetworkManager.EvolutionNumber);
        start();
    }

    private void FixedUpdate() {
        toggle.isOn = NetworkManager.AutomaticAcceleration;
        automaticAcceleration = NetworkManager.AutomaticAcceleration;
        if(automaticAcceleration) {
            if(Time.timeScale < 1.5f) {
                Time.timeScale += 0.003f;
            }
            else if(Time.timeScale < 10f) {
                Time.timeScale += 0.01f;
            }
        }
        currentMaxScore = 0;
        for(int i = 0; i < numberOfAgents; i++) checkAlive(i);
        for(int i = 0; i < numberOfAgents; i++) update(i);
        textMeshScore.text = currentMaxScore.ToString("0");
        textMeshAlive.text = "Alive: " + aliveNumber.ToString();
        restarting = false;
    }

    private void update(int i) {
        if(restarting) return;
        if(!notAlive[i]) {
            scores[i] = getAgentScore(i);
            saveAgentMaxScore(i);
        }
    }

    private void checkAlive(int i) {
        if(restarting) return;
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
        // Debug.Log(aliveNumber);
        if(aliveNumber == 0) {
            restart();
            restarting = true;
            // Debug.Log("restarting");
            // Debug.Log(aliveNumber);
            // menu.TrainAgain();
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
