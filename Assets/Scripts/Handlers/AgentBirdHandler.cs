using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : MonoBehaviour
{
    public GameHandler GameHandler;
    public float gravity = -9.8f;
    public float jumpSpeed = 10f;
    private bool alive;
    private float score;
    private Vector3 speed;

    private SpriteRenderer spriteRenderer;
    private int spriteIndex;
    public Sprite[] sprites;

    private void Awake() {
        alive = true;
        score = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Animate(){
        spriteIndex++;
        if(spriteIndex > sprites.Length - 1) spriteIndex = 0;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void Start() {
        InvokeRepeating(nameof(Animate), 0.15f, 0.1f);
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            speed = Vector3.up * jumpSpeed;
        }
        // if(Input.Touch() > 0) {
        //     Touch touch = Input.GetTouch(0);
        //     if(touch.phase == TouchPhase.Began) {
        //         speed = Vector3.up * jumpSpeed;
        //     }
        // }
        speed.y += gravity * Time.deltaTime*3.25f;
        transform.position += speed * Time.deltaTime/0.7f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Obstacle") {
            if(alive) GameHandler.setDead();
            setDead();
        }
        else if(other.gameObject.tag == "Score") {
            addScore(1f);
        }
    }

    private void setDead() {
        alive = false;
    }

    private void addScore(float sc) {
        if(alive) {
            score += sc;
        }
    }

    public bool getAlive() {
        return alive;
    }

    public float getScore() {
        return score;
    }
}

