using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] GameObject birdPrefab;
    void Start() {
        // Debug.Log("TEST");
        // if(PoolsHandler.prefab != null) return;
        // PoolsHandler.prefab = birdPrefab;
        // PoolsHandler.create();
        // PoolsHandler.play();
    }
    
    public void PlayGame() {
        // Debug.Log(Test.pool.GetPooledObject());
        // GameObject bird = Test.pool.GetPooledObject();
        // bird.SetActive(true);
        // Debug.Log(Test.pool.pooledObjects.Count);
        // PoolsHandler.restart();
        // PoolsHandler.play();
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
