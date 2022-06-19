using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    private float value, bias;
    private float[] weights;

    public float Value {
        get{ return value; }
        set{ this.value = value;}
    }
    public float Bias {
        get{ return bias; }
        set{ bias = value; }
    }
    public float[] Weights{
        get{ return weights; }
        set{ weights = value; }
    }

    public Neuron(int weightsN) {
        value = 0;
        bias = PopulationManager.getRandomWeight();
        weights = new float[weightsN];
        for(int i = 0; i < weights.Length; i++) weights[i] = PopulationManager.getRandomWeight();
    }

    public Neuron(int weightsN, bool bias) {
        value = 0;
        this.bias = 0;
        weights = new float[weightsN];
        if(weightsN == 1) weights[0] = 1;
        else for(int i = 0; i < weights.Length; i++) weights[i] = PopulationManager.getRandomWeight();
    }
}
