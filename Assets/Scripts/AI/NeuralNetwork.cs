using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {
    // private int[] topology = new int[]{4,6,3,1};
    private int[] topology = NetworkManager.Topology;
    private float score, liveTime, fitness;
    private Neuron[][] neuralNetwork;

    public int[] Topology {
        get{return topology;}
        set{topology = value;}
    }

    public float Fitness {
        get{return fitness;}
        set{fitness = value;}
    }
    
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

    public NeuralNetwork(List<float> genome) {
        this.neuralNetwork = new NeuralNetwork().Network;
        if(genome.Count == 0) return;
        genome.Add(0);
        float[] gens = genome.ToArray();
        int id = 0;
        for(int i = 1; i < topology.Length; i++) {
            for(int j = 0; j < topology[i]; j++) {
                for(int k = 0; k < neuralNetwork[i][j].Weights.Length; k++) {
                    neuralNetwork[i][j].Weights[k] = gens[id++];
                }
                neuralNetwork[i][j].Bias = gens[id++];
            }
        }
    }

    public double propagate(float[] input) {
        int i = 0;
        for(int j = 0; j < neuralNetwork[i].Length; j++) neuralNetwork[i][j].Value = input[j];
        for(i = 1; i < neuralNetwork.Length; i++) {
            for(int j = 0; j < neuralNetwork[i].Length; j++) {
                neuralNetwork[i][j].Value = neuralNetwork[i][j].Bias;
                for(int k = 0; k < neuralNetwork[i][j].Weights.Length; k++) {
                    neuralNetwork[i][j].Value += neuralNetwork[i][j].Weights[k] * neuralNetwork[i-1][k].Value;
                }
            }
        }
        return activationSigmoidFunction(neuralNetwork[neuralNetwork.Length-1][0].Value);
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

    private double activationSigmoidFunction(float x) {
        return 1/(1 + System.Math.Exp(-x));
    }

    private double activationReluFunction(float x) {
        return System.Math.Max(0, x);
    }
}