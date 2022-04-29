using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] GameObject birdPrefab;
    void Awake() {
        Application.targetFrameRate = 144;
        Time.timeScale = 1f;
    }
    
    public void PlayGame() {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayAgentGame() {
        SceneManager.LoadScene("AgentScene");
    }

    public void Train() {
        NetworkManager.EvolutionNumber = 0;
        SceneManager.LoadScene("TrainingScene");
    }

    public void TrainAgain() {
        NetworkManager.EvolutionNumber++;
        SceneManager.LoadScene("TrainingScene");
    }

    public void QuitApp() {
        Application.Quit();
    }
}
