using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private MovePipe[] pipes;
    private float liveTime = 0;
    private float[] input;
    private NeuralNetwork network;
    public NeuralNetwork Network{
        get {return network;}
        set {network = value;}
    }

    public void updatedNetworkTime() {
        network.LiveTime = liveTime;
    }

    public void updateNetworkScore(float score) {
        network.Score = score;
    }

    private void Update()  { 
        liveTime += Time.deltaTime;
        speed.y += gravity * 3.25f* Time.deltaTime;
        transform.position += speed/0.7f * Time.deltaTime;
    }

    private void FixedUpdate() {
        pipes = PipesController.getPipes();
        input = new float[5];
        input[0] = transform.position.y;
        input[1] = pipes[0].transform.position.x;
        input[2] = pipes[0].transform.position.y;
        input[3] = pipes[1].transform.position.x;
        input[4] = pipes[1].transform.position.y;
        if(network.propagate(input) > 0.5) {
            jump();
        }
    }
}

