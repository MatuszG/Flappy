using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePipe : MonoBehaviour {
    private float speed = 5f;
    private Rigidbody2D rb;
    
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * Time.deltaTime * Vector3.left);
        // transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
