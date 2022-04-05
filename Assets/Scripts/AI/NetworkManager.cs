using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkManager {
    private const float percentage = 0.1f;
    private static NeuralNetwork[] networks, matingPoolArray;
    private static List<NeuralNetwork> networksList, matingPoolList;
    private static int networksN, evolutionNumber = 0;
    public static int EvolutionNumber {
        get { return evolutionNumber; }
        set { evolutionNumber = value; }
    }
    public static NeuralNetwork[] Networks {
        get{return networks;}
        set{networks = value;}
    }
    public static int NetworksN {
        get{return networksN;}
        set{networksN = value;}
    }

    public static void create() {
        networks = new NeuralNetwork[networksN];
        for(int i = 0; i < networksN; i++) {
            networks[i] = new NeuralNetwork();
        }
    }

    public static void mutate() {
        selection();
        create();
        crossover();
    }

    private static void selection() {
        networksList = new List<NeuralNetwork>(networks);
        matingPoolList = new List<NeuralNetwork>();
        networksList.Sort(delegate(NeuralNetwork x, NeuralNetwork y) {
            if(x.Score >= y.Score && x.LiveTime > y.LiveTime) return -1;
            else if(x.Score <= y.Score && x.LiveTime < y.LiveTime) return 1;
            else return 0;
        });
        // float maxFitness =  networksList[0].Score;
        for(int i = 0; i < networksList.Count; i++) {
            // float fitness = Map(networksList[i].Score, 0, maxFitness, 0, 1);
            // Debug.Log(fitness);
            float fitness = networksList[i].Score * 5 + networksList[i].LiveTime / 3;
            fitness = Mathf.Pow(fitness, 3f);
            int n = (int)Mathf.Floor(fitness * 100);
            Debug.Log(n);
            for(int j = 0; j < n; j++) matingPoolList.Add(networksList[i]);
        }
        Debug.Log(matingPoolList.Count);
        // int index = (int)(networksN * percentage);
        // networksList.RemoveRange(index, networksList.Count - index);
        // networks = networksList.ToArray();
        // index = 2;
        // networksList.RemoveRange(index, networksList.Count - index);
    }

    private static void crossover() {
        int randomRange;
        List<float> firstGenome, secondGenome;
        matingPoolArray = matingPoolList.ToArray();
        networksList = new List<NeuralNetwork>();
        // for(int i = 0; i < networksList.Count; i++) {
        //     if(i > 2) break;
        //     networksList.Add(new NeuralNetwork(networks[0], networksList[i].getGenome()));
        // }
        while (networksList.Count < networksN) { 
            randomRange = RandomRange();
            firstGenome = matingPoolArray[randomId()].getGenome();
            secondGenome = matingPoolArray[randomId()].getGenome();
            networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
            // if(networksList.Count == networksN - 1) {
            //     networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
            // }
            // else {
            //     networksList.Add(genomeCrossoverMutation(firstGenome, secondGenome, randomRange));
            //     networksList.Add(genomeCrossoverMutation(secondGenome, firstGenome, randomRange));
            // }
        }
        networks = networksList.ToArray();
    }

    private static NeuralNetwork genomeCrossoverMutation(List<float> firstGenome, List<float> secondGenome, int range) {
        List<float> newGenome = firstGenome.GetRange(0, range);
        newGenome.AddRange(secondGenome.GetRange(range, secondGenome.Count - range));
        mutation(newGenome);
        NeuralNetwork network = new NeuralNetwork(networks[0], newGenome);
        return network;
    }

    private static void mutation(List<float> genome) {
        for(int i = 0; i < genome.Count; i++) {
            if(Random.Range(0, 1f) > 0.8f) {
                if(!bias(genome[i])) genome[i] = getRandomWeight();
                else genome[i] = getRandomBias();
            }
            // else {
            //     if(!bias(genome[i])) {
            //         genome[i] += Random.Range(-0.05f, 0.05f);
            //         if(!bias(genome[i])) genome[i] = getRandomWeight();
            //     }
            //     else {
            //         genome[i] += Random.Range(-1f, 1f);
            //         if(bias(genome[i])) genome[i] = getRandomBias();
            //     }
            // }
        }
    }

    private static float Map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    private static int randomId() {
        float last = matingPoolArray.Length - 1;
        return (int)Mathf.Round(Random.Range(0, last));
    }

    private static int RandomRange() {
        float last = networks[0].getGenome().Count;
        return (int)Mathf.Round(Random.Range(0, last));
    }

    public static float getRandomWeight() {
        return Random.Range(-1f,1f);
    }

    public static float getRandomBias() {
        if(Random.Range(0, 1f) > 0.5) return Random.Range(2f,10f);
        else return Random.Range(-10f, -2f);
    }

    private static bool bias(float gen) {
        if(gen <= 1 && gen >= -1) {
            return false;
        }
        return true;
    }
}
