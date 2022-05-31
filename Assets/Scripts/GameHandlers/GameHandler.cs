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
    [SerializeField] protected GameObject notificationManager;
    protected GameObject newBird;
    protected int score, maxScore;
    protected bool alive;
    private GameObject gameOverObj;

    private void checkKeybordInput() {
        if (Input.GetKeyDown(KeyCode.Delete)) {
            FileSystem.ResetPlayer();
            notificationManager.GetComponent<NotificationScript>().ShowNotification("Player score has been deleted!");
        }
    }

    private void Start() {
        alive = true;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.01f;
        maxScore = getMaxScore();
        Instantiate(pipeHandler);
        newBird = Instantiate(birdHandler);
    }

    private void FixedUpdate() {
        if(alive) {
            score = getScore();
            scoreText.gameObject.GetComponent<TextMeshProUGUI>().text = score.ToString("0");
            saveMaxScore();
        }
    }

    private void Update() {
        checkKeybordInput();
        if (Input.GetKeyDown(KeyCode.Escape) && !this.alive) {
            GameObject.Find("Panel").GetComponent<GameOverView>().BackToMenu();
        }
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) && !this.alive) {
            GameObject.Find("Panel").GetComponent<GameOverView>().Restart();
        }
    }
    
    public void setDead() {
        alive = false;
        Time.timeScale = 0.00f;
        gameOverObj = Instantiate(gameOver);
    }
    
    private void saveMaxScore() {
        if(maxScore < score) {
            FileSystem.SaveMaxScore(score);
        }
    }

    private int getMaxScore() {
        return FileSystem.GetMaxScore();
    }

    private int getScore() {
        return newBird.gameObject.GetComponent<BirdHandler>().Score;
    }
}