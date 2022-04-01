using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipesController {
    private static List<GameObject> pipes;
    private static GameObject[] pipess;

    public static void restart() {
        pipes = new List<GameObject>();
        pipess = pipes.ToArray();
    }

    public static void clear() {
        if(pipes.Count > 8) {
            pipes.RemoveAt(0);
            pipess = pipes.ToArray();
        }
    }

    public static void addPipe(GameObject pipe) {
        pipes.Add(pipe);
        pipess = pipes.ToArray();
    }
    
    public static GameObject[] getPipes() {
        return pipess;
    }
}
