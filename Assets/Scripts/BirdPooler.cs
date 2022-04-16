using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPooler : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    private GameObject bird;
    private AgentBirdHandler birdHandler;
    public List<GameObject> pool;
    public List<AgentBirdHandler> scriptPool;
    private const int size = 300;
    private Vector3 defaultPosition = new Vector3(0f,1f,0);

    public BirdPooler(BirdPooler pooler) {
        this.pool = pooler.pool;
        this.scriptPool = pooler.scriptPool;
        this.prefab = pooler.prefab;
    }

    void Start() {
        Debug.Log("start");
        pool = new List<GameObject>();
        scriptPool = new List<AgentBirdHandler>();
        while(scriptPool.Count < size) {
            bird = Instantiate(prefab);
            birdHandler = bird.GetComponent<AgentBirdHandler>();
            birdHandler.setActive(false);
            scriptPool.Add(birdHandler);
            // bird.SetActive(false);
            // pool.Add(bird);
        }
        Debug.Log(this);
        // PoolsHandler.start(Instantiate<BirdPooler>(this));
    }

    public void play() {
        Debug.Log("play");
        for(int i = 0; i < scriptPool.Count; i++) {
            scriptPool[i].setPosition(defaultPosition);
            scriptPool[i].setActive(true);
        }
    }

    public void restart() {
        Debug.Log("restart");
        if(scriptPool == null) return;
        for(int i = 0; i < scriptPool.Count; i++) {
            scriptPool[i].setActive(false);
            scriptPool[i].setPosition(defaultPosition);
        }
    }
}
