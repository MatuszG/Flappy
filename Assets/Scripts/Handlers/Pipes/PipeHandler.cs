using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHandler : MonoBehaviour {
    [SerializeField] private GameObject pipe;

    private float timer;
    private float diffTime;
    private int pipesMin = 1;
    private int offsetTime = 10; //12

    private const float xPosDiff = 10.5f; //10
    private const float speedPipe = 5f;
    private float timeMovePipe = xPosDiff/speedPipe;
    private float diffRange;

    private void Start() {
        timer = 0f;
        diffTime = 0f;
        PipesController.restart();
        for(int i = 0; i < pipesMin; i++) {
            addPipe(xPosDiff * i);
        }
    }

    public void Restart() {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject pipe in pipes) {
            pipe.SetActive(false);
            Destroy(pipe);
        }
        Start();
    }

    private void FixedUpdate() {
        timeMovePipe = xPosDiff/(speedPipe);
        diffRange = timeMovePipe/20;
        PipesController.clear();
        if(timer >= diffTime) {
            addPipe(xPosDiff * pipesMin);
        }
        updateTime();
    }

    private void updateTime() {
        timer += Time.deltaTime;
    }

    private void addPipe(float xPos) {
        float range = Random.Range(-6f, 6f);
        GameObject newPipe = Instantiate(pipe, transform.position, Quaternion.identity);
        newPipe.transform.position = transform.position + new Vector3(xPos, range, 0); // (-6f, -6f)
        PipesController.addPipe(newPipe.gameObject);
        float timeDestroy = offsetTime + xPosDiff * pipesMin * diffRange;
        Destroy(newPipe, timeDestroy);
        if(xPos == xPosDiff * pipesMin) {
            timer = 0;
            diffTime = Random.Range(timeMovePipe - diffRange, timeMovePipe + diffRange);
        }
    }
}
