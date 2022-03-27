using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame() {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayAgentGame() {
        SceneManager.LoadScene("AgentScene");
    }

    public void Train() {
        PipesController.EvolutionNumber = 0;
        SceneManager.LoadScene("TrainingScene");
    }

    public void TrainAgain() {
        PipesController.EvolutionNumber++;
        SceneManager.LoadScene("TrainingScene");
    }

    public void QuitApp() {
        Application.Quit();
    }
}
