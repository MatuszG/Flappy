using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHandler : MonoBehaviour {
    [SerializeField] protected GameHandler gameHandler;
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float jumpSpeed = 10f;
    [SerializeField] protected Sprite[] sprites;
    protected bool alive;
    protected int id = -1;
    protected float maxScore;
    protected Rigidbody2D rb;
    protected Vector3 speed;
    protected SpriteRenderer spriteRenderer;
    protected int spriteIndex;

    public int Id {
        get { return id; }
        set { id = value; }
    }
    public bool Alive {
        get { return alive; }
        set { alive = value; }
    }
    protected float score;
    public float Score {
        get { return score; }
        set { score = value; }
    }

    protected void Awake() {
        alive = true;
        score = 0;
        maxScore = FileSystem.GetAgentMaxScore();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Animate() {
        if(id == -1) {
            spriteIndex++;
            if(spriteIndex == sprites.Length) spriteIndex = 0;
            spriteRenderer.sprite = sprites[spriteIndex];
        }
        // spriteIndex++;
        // if(spriteIndex == sprites.Length) spriteIndex = 0;
        // spriteRenderer.sprite = sprites[spriteIndex];
    }

    protected void Start() {
        InvokeRepeating(nameof(Animate), 0.15f, 0.1f);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
            jump();
        }
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began) {
                jump();
            }
        }
        speed.y += gravity * Time.deltaTime*3.25f;
        transform.position += speed * Time.deltaTime/0.7f;
    }

    protected void jump() {
        speed = Vector3.up * jumpSpeed;
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Obstacle" && alive) {
            if(id == -1) gameHandler.setDead();
            setDead();
        }
        else if(other.gameObject.tag == "GroundObstacle" && alive) {
            if(id == -1) gameHandler.setDead();
            setDead();
            if(id != -1) NetworkManager.Networks[id].LiveTime -= 1f;
        }
        else if(other.gameObject.tag == "Score") {
            addScore(1f);
        }
    }

    protected void setDead() {
        alive = false;
    }

    protected void addScore(float sc) {
        if(alive) {
            score += sc;
        }
    }
}
