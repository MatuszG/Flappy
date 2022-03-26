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
    protected GameObject newBird, newEndGame;
    protected float score, maxScore;
    protected bool alive;

    public void setDead() {
        alive = false;
        Time.timeScale = 0f;
        newEndGame = Instantiate(gameOver);
    }

    private void OnEnable() {
        maxScore = getMaxScore();
        Time.timeScale = 1f;
        alive = true;
        newBird = Instantiate(birdHandler);
        Instantiate(pipeHandler);
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
        return newBird.gameObject.GetComponent<BirdHandler>().getScore();
    }

    private void Update() {
        if(alive) {
            score = getScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveMaxScore();
        }
    }
}