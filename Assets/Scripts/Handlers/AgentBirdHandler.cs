using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private List<GameObject> pipes;
    private float random;
    // Update is called once per frame
    private void Update()
    {   
        random = Random.Range(0f,3f);
        // Debug.Log(random);
        if(random > 2.97) {
            jump();
        }
        speed.y += gravity * Time.deltaTime*3.25f;
        transform.position += speed * Time.deltaTime/0.7f;
    }
}

