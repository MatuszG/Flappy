using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {
    private int[] topology = new int[]{2,3,1};
    private Neuron[][] neuralNetwork;
    public NeuralNetwork() {
        neuralNetwork = new Neuron[topology.Length][];
        for(int i = 0; i < topology.Length; i++) {
            neuralNetwork[i] = new Neuron[topology[i]];
            for(int j = 0; j < topology[i]; j++) {
                if(i == 0) neuralNetwork[i][j] = new Neuron(1,false);
                else if(i == topology.Length-1) neuralNetwork[i][j] = new Neuron(topology[i-1], false);
                else neuralNetwork[i][j] = new Neuron(topology[i-1]);
            }
        }
    }

    public double propagate(float[] input) {
        // float[] input = {1,2};
        for(int i = 0; i < neuralNetwork.Length; i++) {
            if(i == 0) for(int j = 0; j < neuralNetwork[i].Length; j++) neuralNetwork[i][j].Value = input[j];
            else {
                for(int j = 0; j < neuralNetwork[i].Length; j++) {
                    neuralNetwork[i][j].Value = neuralNetwork[i][j].Bias;
                    for(int k = 0; k < neuralNetwork[i-1].Length; k++) {
                        neuralNetwork[i][j].Value += neuralNetwork[i][j].Weights[k] * neuralNetwork[i-1][k].Value;
                    }
                }
            }
        }
        return activationFunction(neuralNetwork[neuralNetwork.Length-1][0].Value);
    }

    private double activationFunction(float x) {
        return 1/(1 + System.Math.Exp(-x));
    }

    public void print() {
        neuralNetwork[1][0].print();
        // Debug.Log(neuralNetwork[0][0]);
        // Debug.Log(neuralNetwork[1][0]);
        // Debug.Log(neuralNetwork[1][1]);
        // Debug.Log(neuralNetwork[2][0]);
    }
}