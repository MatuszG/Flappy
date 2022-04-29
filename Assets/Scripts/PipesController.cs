using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipesController {
    private static List<MovePipe> pipeHandlers;
    private static List<MovePipe> bestPipeHandlers;

    public static void restart() {
        pipeHandlers = new List<MovePipe>();
        bestPipeHandlers = new List<MovePipe>();
    }

    public static void clear() {
        if(pipeHandlers.Count > 5) {
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
        return bestPipeHandlers.ToArray();
    }

    private static void updateBestPipes() {
        bestPipeHandlers = pipeHandlers;
        if(bestPipeHandlers == null) return;
        bestPipeHandlers.Sort(delegate(MovePipe a, MovePipe b){
            if(a.transform.position.x < b.transform.position.x) return -1;
            else if(a.transform.position.x >= b.transform.position.x) return 1;
            else return 0;
        });
        // if bird is after pipe, look for another
        if(bestPipeHandlers[0].transform.position.x < 0.5) bestPipeHandlers.RemoveAt(0);
    }
}
