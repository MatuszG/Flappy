using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private GameObject[] pipes;
    private float random;
    private NeuralNetwork network;
    public NeuralNetwork Network{
        get {return network;}
        set {network = value;}
    }
    private float[] input;
    private void Update()  { 
        pipes = gameHandler.getPipes();
        input = new float[4];
        
        if(network != null && network.propagate(input) > 0.5) {
            jump();
        }
        speed.y += gravity * 3.25f* Time.deltaTime;
        transform.position += speed/0.7f * Time.deltaTime;
    }

    // private void Update() {
        
    // }
}

