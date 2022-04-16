using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipesController {
    private static List<MovePipe> pipeHandlers;
    private static MovePipe[] bestPipeHandlers;
    private static float[] bestPipes;

    public static void restart() {
        pipeHandlers = new List<MovePipe>();
        bestPipeHandlers = new MovePipe[2];
        bestPipes = new float[]{Mathf.Infinity, Mathf.Infinity};
    }

    public static void clear() {
        if(pipeHandlers.Count > 8) {
            pipeHandlers.RemoveAt(0);
            updateBestPipes();
        }
    }

    public static void addPipe(GameObject pipe) {
        pipeHandlers.Add(pipe.GetComponent<MovePipe>());
        updateBestPipes();
    }
    
    public static MovePipe[] getPipes() {
        updateBestPipes();
        return bestPipeHandlers;
    }

    private static void updateBestPipes() { // do poprawy
        if(bestPipeHandlers == null) return;
        bestPipes = new float[]{Mathf.Infinity, Mathf.Infinity};
        for(int i = 0; i < bestPipeHandlers.Length; i++) {
            if(bestPipeHandlers[i] != null && bestPipeHandlers[i].transform.position.x < 0.6) {
                bestPipes[i] = Mathf.Infinity;
            }
        }
        foreach(MovePipe pipe in pipeHandlers) {
            if(pipe.transform.position.x >= 0.1) {
                if(bestPipes[0] >= bestPipes[1] && pipe.transform.position.x < bestPipes[0]) {
                    bestPipes[0] = pipe.transform.position.x;
                    bestPipeHandlers[0] = pipe;
                }
                else if(bestPipes[1] > bestPipes[0] && pipe.transform.position.x < bestPipes[1]) {
                    bestPipes[1] = pipe.transform.position.x;
                    bestPipeHandlers[1] = pipe;
                }
            }
        }
    }
}
