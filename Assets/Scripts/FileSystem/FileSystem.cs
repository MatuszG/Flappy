using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

// path - C:/Users/garba/AppData/LocalLow/DefaultCompany/Flappy/

// @ -1 means File not found
public static class FileSystem {
    static string pathScore = Application.persistentDataPath + "/playerMaxScore.bin";
    static string pathAgentScore = Application.persistentDataPath + "/agentMaxScore.bin";
    static string pathAgentPolicy = Application.persistentDataPath + "/agentPolicy.bin";

    public static void printPath() {
        Debug.Log(pathAgentPolicy);
    }

    public static void ResetPlayer() {
        File.Delete(pathScore);
    }

    public static void ResetAgentScore() {
        File.Delete(pathAgentScore);
    }

    public static void ResetAgentPolicy() {
        File.Delete(pathAgentPolicy);
    }

    public static void SaveMaxScore(float score) {
        float data = GetMaxScore();
        if(data < score) {
            SaveScore(pathScore, score);
        }
    }

    public static void SaveAgentMaxScore(float score) {
        float data = GetAgentMaxScore();
        if(data < score) {
            SaveScore(pathAgentScore, score);
        }
    }

    public static void SaveAgentPolicy(List<float> genome) {
        BinaryFormatter formatter = new BinaryFormatter();
        AgentPolicy data = new AgentPolicy(genome, NetworkManager.Topology);
        FileStream stream = new FileStream(pathAgentPolicy, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static List<float> GetAgentPolicy() {
        if(File.Exists(pathAgentPolicy)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(pathAgentPolicy, FileMode.Open);
            AgentPolicy data = formatter.Deserialize(stream) as AgentPolicy;
            stream.Close();
            if(data.topology == NetworkManager.Topology) {
                return data.genome;
            }
            return data.genome;
        }
        return new List<float>();
    }

    public static float GetMaxScore() {
        float data = LoadScore(pathScore);
        if(data == -1f) return 0;
        return data;
    }

    public static float GetAgentMaxScore() {
        float data = LoadScore(pathAgentScore);
        if(data == -1f) return 0;
        return data;
    }

    private static void SaveScore(string path, float score) {
        BinaryFormatter formatter = new BinaryFormatter();
        PlayerScore data = new PlayerScore(score);
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    private static float LoadScore(string path) {
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerScore data = formatter.Deserialize(stream) as PlayerScore;
            stream.Close();
            return data.score;
        }
        else {
            return -1f;
        }
    }

}
