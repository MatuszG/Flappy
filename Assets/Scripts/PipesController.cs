using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipesController {
    private static List<GameObject> pipes;
    private static int evolutionNumber = 0;
    public static int EvolutionNumber {
        get { return evolutionNumber; }
        set { evolutionNumber = value; }
    }

    public static void restart() {
        pipes = new List<GameObject>();
    }

    public static void clear() {
        if(pipes.Count > 8) {
            pipes.RemoveAt(0);
        }
    }

    public static void addPipe(GameObject pipe) {
        pipes.Add(pipe);
    }
    
    public static List<GameObject> getPipes() {
        return pipes;
    }
}
