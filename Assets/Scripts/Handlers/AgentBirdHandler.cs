using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private List<GameObject> pipes;
    private float random;

    private void Update()  {  
        random = Random.Range(0f,1f);
        if(random > 0.98) {
            jump();
        }
        speed.y += gravity * 3.25f* Time.deltaTime;
        transform.position += speed/0.7f * Time.deltaTime;
    }
}

