using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHandler : MonoBehaviour {
    [SerializeField] private GameObject pipe;

    private float timer;
    private float diifTime = 0;
    private int pipesMin = 1;
    private int offsetTime = 22; //12

    private const int xPosDiff = 10;
    private const float speedPipe = 5f;
    private float timeMovePipe = xPosDiff/speedPipe;
    private float diffRange;

    private void Start() {
        PipesController.restart();
        timer = 0f;
        for(int i = 0; i < pipesMin; i++) {
            addPipe(xPosDiff * i);
        }
    }

    public void Restart() {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject pipe in pipes) {
            pipe.SetActive(false);
            // Destroy(pipe);
        }
        Start();
    }

    private void Update() {
        // timeMovePipe = xPosDiff/(speedPipe*Time.timeScale);
        timeMovePipe = xPosDiff/(speedPipe);
        diffRange = timeMovePipe/20;
        PipesController.clear();
        if(timer >= diifTime) addPipe(xPosDiff * pipesMin);
        updateTime();
    }

    private void updateTime() {
        timer += Time.deltaTime;
    }

    private void addPipe(int xPos) {
        GameObject newPipe = Instantiate(pipe, transform.position, Quaternion.identity);
        newPipe.transform.position = transform.position + new Vector3(xPos, Random.Range(-6f, 6f), 0); // (-6f, -6f)
        PipesController.addPipe(newPipe.gameObject);
        float timeDestroy = offsetTime + xPosDiff * pipesMin * diffRange;
        Destroy(newPipe, timeDestroy);
        if(xPos == xPosDiff * pipesMin) {
            timer = 0;
            diifTime = Random.Range(timeMovePipe - diffRange, timeMovePipe + diffRange);
        }
    }
}
