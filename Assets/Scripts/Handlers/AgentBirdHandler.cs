using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private MovePipe[] pipes;
    private float liveTime = 0;
    private float[] input = new float[4];
    private bool active = false;
    private Vector3 defaultPos = new Vector3(0,7f,0);
    private NeuralNetwork network;

    public NeuralNetwork Network {
        get {return network;}
        set {network = value;}
    }

    public void saveAgentPolicy() {
        FileSystem.SaveAgentPolicy(network.getGenome());
    }

    public void getAgentPolicy() {
        network = new NeuralNetwork(FileSystem.GetAgentPolicy());
    }

    public void updatedNetworkTime() {
        network.LiveTime = liveTime;
    }

    public void updateNetworkScore(float score) {
        network.Score = score;
    }

    public void setPosition(Vector3 pos) {
        this.transform.position = pos;
    }

    public void setOn(bool active) {
        this.active = active;
    }

    public void setActive(bool active) {
        this.gameObject.SetActive(active);
    }

    public void restart(int i) {
        alive = true;
        liveTime = 0f;
        score = 0;
        maxScore = FileSystem.GetAgentMaxScore();
        this.transform.position = defaultPos;
        this.active = true;
        this.gameObject.SetActive(true);
        Network = NetworkManager.Networks[i];
    }

    private void Update() {
        // Debug.Log(liveTime);
        return;
    }

    private void FixedUpdate()  { 
        if(!active) return;
        // if(pipes == null) { // to optimize <HAVE TO>
        //     pipes = PipesController.getPipes();
        // }
        pipes = PipesController.getPipes();
        if(pipes == null) return;
        if(score > maxScore) {
            maxScore = score;
            FileSystem.SaveAgentPolicy(network.getGenome());
        }
        input[0] = Map(transform.position.y, -0.5f, 0.75f, 0 , 1f);
        input[1] = Map(speed.y, -40f, 10f, -4f, 1f);
        input[2] = Map(pipes[0].transform.position.x, 0, 20f, 0, 1f);
        input[3] = Map(pipes[0].transform.position.y, -6f, 6f, -1f, 1f);
        // input[4] = pipes[1].transform.position.x;
        // input[5] = pipes[1].transform.position.y;
        if(network != null && network.propagate(input) > 0.5) { // CHECK IF CAN OPTIMIZE
            jump();
        }
        liveTime += Time.deltaTime;
        speed.y += gravity * 3.25f* Time.deltaTime;
        rb.MovePosition(transform.position + speed/0.7f * Time.deltaTime); // CHECK IF CAN OPTIMIZE
    }

    private float Map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}

