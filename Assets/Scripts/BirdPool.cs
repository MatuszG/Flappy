using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPool : MonoBehaviour {
    public static BirdPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject[] pooledObjectsArray;
    public List<AgentBirdHandler> scriptedObjects;
    public GameObject objectToPool;
    private int amountToPool = 1000;
    private Vector3 defaultPosition = new Vector3(0f,2f,0);

    void Awake() {
        SharedInstance = this;
    }

    void Start() {
        pooledObjects = new List<GameObject>();
        scriptedObjects = new List<AgentBirdHandler>();
        GameObject bird;
        AgentBirdHandler birdHandler;
        for(int i = 0; i < amountToPool; i++) {
            bird = Instantiate(objectToPool, defaultPosition, Quaternion.identity);
            birdHandler = bird.GetComponent<AgentBirdHandler>();
            pooledObjects.Add(bird);
            scriptedObjects.Add(birdHandler);
        }
        for(int i = 0; i < amountToPool; i++) {
            scriptedObjects[i].setActive(false);
            // scriptedObjects[i].setOn(false);
        }
        pooledObjectsArray = pooledObjects.ToArray();
        Test.pool = SharedInstance;
    }

    public GameObject GetPooledObject(int id) {
        for(int i = 0; i < amountToPool; i++) {
            // if(!pooledObjects[i].activeInHierarchy) 
                // return pooledObjects[i];
            if(i==id) return pooledObjectsArray[i];
        }
        return null;
    }

}
