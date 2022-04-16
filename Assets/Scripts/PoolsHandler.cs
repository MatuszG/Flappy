using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolsHandler {
    public static GameObject prefab;
    public static List<GameObject> pool;
    public static List<AgentBirdHandler> scriptPool;
    private static GameObject bird;
    private static AgentBirdHandler birdHandler;
    private const int size = 5;
    private static Vector3 defaultPosition = new Vector3(0f,7f,0);

    public static void create() {
        if(prefab == null || scriptPool != null) return;
        pool = new List<GameObject>();
        scriptPool = new List<AgentBirdHandler>();
        while(scriptPool.Count < size) {
            // Debug.Log(UnityEngine.Object.Instantiate(prefab));
            bird = UnityEngine.Object.Instantiate(prefab, defaultPosition, Quaternion.identity);
            birdHandler = bird.GetComponent<AgentBirdHandler>();
            pool.Add(bird);
            scriptPool.Add(birdHandler);
        }
        for(int i = 0; i < scriptPool.Count; i++) {
            scriptPool[i].setActive(false);
        }
    }

    public static void play() {
        Debug.Log(pool.Count);
        Debug.Log("play");
        for(int i = 0; i < scriptPool.Count; i++) {
            scriptPool[i] = pool[i].GetComponent<AgentBirdHandler>();
            scriptPool[i].setActive(true);
        }
    }

    public static void restart() {
        Debug.Log("restart");
        if(scriptPool == null) return;
        for(int i = 0; i < scriptPool.Count; i++) {
            scriptPool[i].setActive(false);
            scriptPool[i].setPosition(defaultPosition);
        }
    }

    // public static bool isPool() {
    //     if(pool == null) return false;
    //     return true;
    // }

    // public static void start(BirdPooler pooler) {
    //     if(pool != null) return;
    //     pool = pooler;
    //     Debug.Log(pool);
    // }

    // public static void play() {
    //     if(pool == null) return;
    //     pool.play();
    // }

    // public static void restart() {
    //     Debug.Log(pool);
    //     if(pool == null) return;
    //     pool.restart();
    // }
}
