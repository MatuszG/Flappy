using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private MovePipe[] pipes;
    private float liveTime = 0;
    private float[] input;
    private bool active = false;
    private NeuralNetwork network;

    private Vector2 sped;

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
        // this.gameObject.SetActive(active);
        this.active = active;
    }

    public void setActive(bool active) {
        this.gameObject.SetActive(active);
    }

    private void Update() {
        return;
    }

    private void FixedUpdate()  { 
        if(!active) return;
        pipes = PipesController.getPipes();
        if(pipes == null) return;
        if(score > maxScore) {
            maxScore = score;
            FileSystem.SaveAgentPolicy(network.getGenome());
        }
        input = new float[4];
        input[0] = Map(transform.position.y, -0.5f, 0.75f, 0 , 1f);
        input[1] = Map(speed.y, -40f, 10f, -4f, 1f);
        input[2] = Map(pipes[0].transform.position.x, 0, 20f, 0, 1f);
        input[3] = Map(pipes[0].transform.position.y, -6f, 6f, -1f, 1f);
        // input[4] = pipes[1].transform.position.x;
        // input[5] = pipes[1].transform.position.y;
        if(network != null && network.propagate(input) > 0.5) {
            jump();
        }

        // speed.y += gravity * 3.25f* Time.deltaTime;
        // Debug.Log(rb);
        // Debug.Log(rb.velocity.y);
        liveTime += Time.deltaTime;
        speed.y += gravity * 3.25f* Time.deltaTime;
        rb.MovePosition(transform.position + speed/0.7f * Time.deltaTime);
        // transform.position += speed/0.7f * Time.deltaTime;
        // sped.y += gravity * 3.25f* Time.deltaTime;
        // rb.velocity = sped;
        // rb.position = sped/0.7f * Time.deltaTime;
        // Debug.Log(rb.velocity.y);
    }

    private float Map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    // private void FixedUpdate() {
    //     pipes = PipesController.getPipes();
    //     input = new float[6];
    //     input[0] = transform.position.y;
    //     input[1] = speed.y;
    //     input[2] = pipes[0].transform.position.x;
    //     input[3] = pipes[0].transform.position.y;
    //     // input[4] = pipes[1].transform.position.x;
    //     // input[5] = pipes[1].transform.position.y;
    //     if(network.propagate(input) > 0.5) {
    //         jump();
    //     }
    // }
}

