using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {
    private int[] topology = new int[]{5,8,1};
    private float score, liveTime;
    private Neuron[][] neuralNetwork;

    public float LiveTime {
        get{return liveTime;}
        set{liveTime = value;}
    }
    public float Score {
        get{return score;}
        set{score = value;}
    }
    public Neuron[][] Network {
        get{return neuralNetwork;}
        set{neuralNetwork = value;}
    }

    public NeuralNetwork() {
        neuralNetwork = new Neuron[topology.Length][];
        for(int i = 0; i < topology.Length; i++) {
            neuralNetwork[i] = new Neuron[topology[i]];
            for(int j = 0; j < topology[i]; j++) {
                if(i == 0) neuralNetwork[i][j] = new Neuron(1,false);
                else if(i == topology.Length - 1) neuralNetwork[i][j] = new Neuron(topology[i - 1], false);
                else neuralNetwork[i][j] = new Neuron(topology[i-1]);
            }
        }
    }

    public NeuralNetwork(NeuralNetwork network, List<float> genome) {
        this.neuralNetwork = network.Network;
        float[] gens = genome.ToArray();
        int id = 0;
        for(int i = 1; i < topology.Length - 1; i++) {
            for(int j = 0; j < topology[i]; j++) {
                for(int k = 0; k < neuralNetwork[i][j].Weights.Length; k++) {
                    neuralNetwork[i][j].Weights[k] = gens[id++];
                }
                neuralNetwork[i][j].Bias = gens[id++];
            }
        }
    }

    public double propagate(float[] input) {
        if(neuralNetwork == null) return 0;
        for(int i = 0; i < neuralNetwork.Length; i++) {
            if(i == 0) for(int j = 0; j < neuralNetwork[i].Length; j++) neuralNetwork[i][j].Value = input[j];
            else {
                for(int j = 0; j < neuralNetwork[i].Length; j++) {
                    neuralNetwork[i][j].Value = neuralNetwork[i][j].Bias;
                    for(int k = 0; k < neuralNetwork[i-1].Length; k++) {
                        neuralNetwork[i][j].Value += neuralNetwork[i][j].Weights[k] * neuralNetwork[i-1][k].Value;
                    }
                    neuralNetwork[i][j].Value = (float)activationSigmoidFunction(neuralNetwork[i][j].Value);
                }
            }
        }
        // Debug.Log(neuralNetwork[neuralNetwork.Length-1][0].Value);
        return activationFunction(neuralNetwork[neuralNetwork.Length-1][0].Value);
    }

    public List<float> getGenome() {
        List<float> genome = new List<float>();
        if(neuralNetwork == null) return genome;
        for(int i = 1; i < topology.Length; i++) {
            for(int j = 0; j < topology[i]; j++) {
                for(int k = 0; k < neuralNetwork[i][j].Weights.Length; k++) {
                    genome.Add(neuralNetwork[i][j].Weights[k]);
                }
                genome.Add(neuralNetwork[i][j].Bias);
            }
        }
        genome.RemoveAt(genome.Count - 1);
        return genome;
    }

    private double activationFunction(float x) {
        return 1/(1 + System.Math.Exp(-x));
    }

    private double activationSigmoidFunction(float x) {
        return System.Math.Max(0, x);
    }
}