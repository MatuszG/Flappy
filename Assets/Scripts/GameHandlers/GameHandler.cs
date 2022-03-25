using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour
{
    public GameObject gameOver;
    public GameObject birdHandler;
    public GameObject pipeHandler;
    public GameObject scoreText;
    private GameObject newBird;
    private GameObject newEndGame;
    private GameObject newPipeHandler;
    private float score;
    private bool alive;

    private void OnEnable() {
        Time.timeScale = 1f;
        alive = true;
        newBird = Instantiate(birdHandler);
        newPipeHandler = Instantiate(pipeHandler);
    }

    public void setDead(){
        alive = false;
        Time.timeScale = 0f;
        newEndGame = Instantiate(gameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if(alive) {
            score = newBird.gameObject.GetComponent<BirdHandler>().getScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
        }
        else {
            Destroy(newBird.gameObject);
        }
    }
}