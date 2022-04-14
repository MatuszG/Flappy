using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPooler : MonoBehaviour {
    [SerializeField] private GameObject prefab;
    private List<GameObject> pool;
    private const int size = 150;
    // Start is called before the first frame update
    void Start()
    {
        pool = new List<GameObject>();
        while(pool.Count < 150) {
            
        }
    }
}
