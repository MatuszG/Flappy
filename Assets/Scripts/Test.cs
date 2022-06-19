using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

// C:\Users\garba\AppData\LocalLow\DefaultCompany\Flappy

public static class Test {
    private static int[] populationSize = new int[]{800, 600, 500, 250, 100};
    // private static int[] populationSize = new int[]{1500, 1250, 1000};
    private static float[] averages = new float[]{1, 5, 10, 15, 30, 50, 75, 100};
    private static float[] rates = new float[]{1f, 0.75f, 0.5f, 0.25f, 0.1f};
    private static string path = Application.persistentDataPath + "/Individual/";
    private static string[] info = {
        $"# Mutate ratio: {PopulationManager.MutateRatio} ",
        $"# Learning rate: {PopulationManager.LearningRate} ",
        $"# Topology: {PopulationManager.Topology[0]}|{PopulationManager.Topology[1]}|{PopulationManager.Topology[2]}",
        "#Ev\tM_Score\tMax_Live_Time\tMax_Fitness\tAvg_1\tAvg_5\tAvg_10\tAvg_15\tAvg_30\tAvg_50\tAvg_75\tAvg_100"
    };
    private static List<string> data;
    private static List<float> liveTimes, fitnesses;
    private static List<int> scores;
    private static int testId, populationId, scoreUpperBound, maxProbes, ratesId = 0;

    public static int ScoreUpperBound {
        get { return scoreUpperBound; }
        set { scoreUpperBound = value; }
    }

    public static float Sum(this IEnumerable<float> source) {
        return source.Aggregate((x, y) => x + y);
    }

    public static void start() {
        if(ratesId == rates.Length) {
            Application.Quit();
        }
        testId = 0;
        populationId = 0;
        scoreUpperBound = 10000;
        maxProbes = 20;
        data = new List<string>(info);
        PopulationManager.ParentsOffSprings = false;
        PopulationManager.LearningRate = rates[ratesId++];
        PopulationManager.MaxPopulationScore = 0;
        PopulationManager.NetworksN = populationSize[testId++];
        PopulationManager.AutomaticAcceleration = true;
    }

    public static void increment() {
        save();
        populationId++;
        if(populationId == maxProbes) {
            populationId = 0;
            if(testId == populationSize.Length) start();
            else PopulationManager.NetworksN = populationSize[testId++];
        }
        data = new List<string>(info);
        PopulationManager.MaxPopulationScore = 0;
    }

    public static float calculateAvg(int[] data, float avg) {
        int average = 0;
        float size = data.Length * avg/100;
        for(int i = 0; i < size; i++) {
            average += data[i];
        }
        return average/size;
    }

    public static string writeAvg() {
        string averagesInfo = "";
        foreach(float avg in averages) {
            averagesInfo += calculateAvg(scores.ToArray(), avg) + "\t";
        }
        return averagesInfo;
    }

    public static void calculate() {
        scores = new List<int>();
        liveTimes = new List<float>();
        fitnesses = new List<float>();
        for(int i = 0; i < PopulationManager.Networks.Length; i++) {
            scores.Add(PopulationManager.Networks[i].Score);
            liveTimes.Add(PopulationManager.Networks[i].LiveTime);
            fitnesses.Add((float)PopulationManager.Networks[i].Fitness);
        }
        scores.Sort((a, b) => (b.CompareTo(a)));
        liveTimes.Sort((a, b) => (b.CompareTo(a)));
        fitnesses.Sort((a, b) => (b.CompareTo(a)));
        addData();
    }

    public static void addData() {
        string evolutionData =
            $"{PopulationManager.EvolutionNumber-1}\t{scores.First()}\t" +
            $"{liveTimes.First()}\t{fitnesses.First()}\t" + writeAvg();
        data.Add(evolutionData);
    }

    public static void save() {
        string filePath = path + populationId + "_data_" + populationSize[testId-1] + $"_{PopulationManager.EvolutionNumber-1}" + $"L_{PopulationManager.LearningRate}" + ".txt";
        using (StreamWriter sw = File.CreateText(filePath)) {
            foreach(string line in data) {
                sw.WriteLine(line);
            }
        }
    }

}
