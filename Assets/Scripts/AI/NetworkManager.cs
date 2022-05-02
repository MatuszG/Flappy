using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.Distributions;
public static class NetworkManager {
    private static NeuralNetwork[] networks;
    private static List<NeuralNetwork> networksList;
    private static int evolutionNumber = 0, networksN = 275, maxPopulationScore = 0;
    private static int[] topology = new int[]{4,7,1};
    private static float sumFitness, mutateRatio = 0.1f, learningRate = 0.2f;
    private static bool automaticAcceleration = false, parentOffSprings = false, continueTraining = true;

    public static float LearningRate {
        get { return learningRate; }
        set { learningRate = value; }
    }

    public static float MutateRatio {
        get { return mutateRatio; }
        set { mutateRatio = value; }
    }

    public static bool ParentOffSprings {
        get { return parentOffSprings; }
        set { parentOffSprings = value; }
    }

    public static bool AutomaticAcceleration {
        get { return automaticAcceleration; }
        set { automaticAcceleration = value; }
    }

    public static NeuralNetwork[] Networks {
        get{return networks;}
        set{networks = value;}
    }

    public static int[] Topology {
        get{return topology;}
        set{topology = value;}
    }

    public static int EvolutionNumber {
        get { return evolutionNumber; }
        set { evolutionNumber = value; }
    }

    public static int NetworksN {
        get{return networksN;}
        set{networksN = value;}
    }

    public static int MaxPopulationScore {
        get{return maxPopulationScore;}
        set{maxPopulationScore = value;}
    }

    public static void create() {
        networks = new NeuralNetwork[networksN];
        for(int i = 0; i < networksN; i++) {
            networks[i] = new NeuralNetwork();
        }
    }

    public static void savePopulation() {
        
    }

    public static void evolve() {
        selection();
        if(parentOffSprings) parentsCrossover();
        else crossover();
        // if(continueTraining) savePopulation();
    }

    private static void selection() {
        networksList = new List<NeuralNetwork>(networks);
        networksList.Sort(delegate(NeuralNetwork x, NeuralNetwork y) {
            if(x.Score >= y.Score && x.LiveTime > y.LiveTime) return -1;
            else if(x.Score <= y.Score && x.LiveTime < y.LiveTime) return 1;
            else return 0;
        });
        sumFitness = 0;
        for(int i = 0; i < networksList.Count; i++) {
            networksList[i].Fitness = Mathf.Pow(networksList[i].LiveTime + networksList[i].Score, 6);
            sumFitness += networksList[i].Fitness;
        }
        for(int i = 0; i < networksList.Count; i++) {
            networksList[i].Fitness = networksList[i].Fitness/sumFitness;
        }
        networks = networksList.ToArray();
    }

    private static List<float> poolSelection() {
        float random = Random.Range(0, 1f);
        int i;
        float sum = 0;
        for(i = 0; i < networks.Length; i++) {
            random -= networks[i].Fitness;
            sum += networks[i].Fitness;
            if(random <= 0) {
                return networks[i].getGenome();
            } 
        }
        return networks[i-1].getGenome();
    }

    private static void savingBests() {
        // Saving the best species
        int i = 0;
        List<float> genome = networks[i++].getGenome();
        networksList.Add(new NeuralNetwork(genome));
        while (networksList.Count < networksN / 100f) { 
            genome = networks[i++].getGenome();
            networksList.Add(new NeuralNetwork(genome));
        }
    }

    private static void crossover() {
        networksList = new List<NeuralNetwork>();
        List<float> genome;
        savingBests();
        // Crossover (mutation) for pool selected species
        while (networksList.Count < networksN) { 
            genome = poolSelection();
            mutation(genome);
            networksList.Add(new NeuralNetwork(genome));
        }
        networks = networksList.ToArray();
    }

    private static void parentsCrossover() {
        int randomRange;
        List<float> firstGenome, secondGenome;
        networksList = new List<NeuralNetwork>();
        // Saving the best species
        savingBests();
        // Crossover (mutation) for pool selected species
        while (networksList.Count < networksN) { 
            randomRange = RandomRange();
            firstGenome = poolSelection();
            secondGenome = poolSelection();
            networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
            if(networksList.Count == networksN - 1) {
                networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
            }
            else {
                networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
                networksList.Add(genomeCrossoverMutation(secondGenome, firstGenome, randomRange));
            }
        }
        networks = networksList.ToArray();
    }

    private static NeuralNetwork genomeCrossoverMutation(List<float> firstGenome, List<float> secondGenome, int range) {
        List<float> newGenome = firstGenome.GetRange(0, range);
        newGenome.AddRange(secondGenome.GetRange(range, secondGenome.Count - range));
        mutation(newGenome);
        NeuralNetwork network = new NeuralNetwork(newGenome);
        return network;
    }

    private static void mutation(List<float> genome) {
        for(int i = 0; i < genome.Count; i++) {
            if(Random.Range(0, 1f) > 1 - mutateRatio) {
                genome[i] += randomGaussian() * learningRate;
            }
        }
    }

    private static float randomGaussian() {
        double mean = 0;
        double stdDev = 1;
        Normal normalDist = new Normal(mean, stdDev);
        return (float)normalDist.Sample();
    }

    private static float Map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private static int RandomRange() {
        float last = networks[0].getGenome().Count;
        return (int)Mathf.Round(Random.Range(0, last));
    }

    public static float getRandomWeight() {
        return Random.Range(-1f,1f);
    }

    public static float getWeightOffset() {
        return Random.Range(-0.5f,0.5f);
    }

    public static float getRandomBias() {
        if(Random.Range(0, 1f) > 0.5) return Random.Range(10f,20f);
        else return Random.Range(-20f, -10f);
    }

    public static float getBiasOffset() {
        if(Random.Range(0, 1f) > 0.5) return Random.Range(1.5f,5f);
        else return Random.Range(-5f, -1.5f);
    }

    private static bool bias(float gen) {
        if(gen <= 1 && gen >= -1) {
            return false;
        }
        return true;
    }
}
