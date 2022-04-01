using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    private float value;
    private float bias;
    private float[] weights;
    public float Value {
        get{return value;}
        set{this.value = value;}
    }
    public float Bias {
        get{return bias;}
        set{bias = value;}
    }
    public float[] Weights{
        get{return weights;}
        set{weights = value;}
    }

    public Neuron(int weightsN) {
        bias = getRandomBias();
        weights = new float[weightsN];
        for(int i = 0; i < weights.Length; i++) weights[i] = getRandomWeight();
    }
    public Neuron(int weightsN, bool bias) {
        this.bias = 0;
        weights = new float[weightsN];
        if(weightsN == 1) weights[0] = 1;
        else for(int i = 0; i < weights.Length; i++) weights[i] = getRandomWeight();
    }
    public void print() {
        Debug.Log(bias);
        Debug.Log(weights[0]);
    }
    private float getRandomWeight() {
        return Random.Range(-1f,1f);
    }
    private float getRandomBias() {
        return Random.Range(-10f,10f);
    }
}