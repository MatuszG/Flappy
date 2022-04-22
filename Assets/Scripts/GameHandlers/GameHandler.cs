using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour {
    [SerializeField] protected GameObject gameOver;
    [SerializeField] protected GameObject birdHandler;
    [SerializeField] protected GameObject pipeHandler;
    [SerializeField] protected GameObject scoreText;
    protected GameObject newBird;
    protected float score, maxScore;
    protected bool alive;

    public void setDead() {
        alive = false;
        Time.timeScale = 0f;
        Instantiate(gameOver);
    }

    private void Start() {
        alive = true;
        Time.timeScale = 1f;
        maxScore = getMaxScore();
        Instantiate(pipeHandler);
        newBird = Instantiate(birdHandler);
    }

    private void saveMaxScore() {
        if(maxScore < score) {
            FileSystem.SaveMaxScore(score);
        }
    }

    private float getMaxScore() {
        return FileSystem.GetMaxScore();
    }

    private float getScore() {
        return newBird.gameObject.GetComponent<BirdHandler>().Score;
    }

    private void FixedUpdate() {
        if(alive) {
            score = getScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveMaxScore();
        }
    }
}