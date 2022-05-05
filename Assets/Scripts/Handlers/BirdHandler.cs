using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHandler : MonoBehaviour {
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] protected float jumpSpeed = 10f;
    [SerializeField] protected Sprite[] sprites;
    protected int id, spriteIndex;
    protected bool alive;
    protected float score, maxScore;
    protected Vector3 speed;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    private GameHandler gameHandler;

    public int Id {
        get { return id; }
        set { id = value; }
    }
    
    public float Score {
        get { return score; }
        set { score = value; }
    }

    public bool Alive {
        get { return alive; }
        set { alive = value; }
    }

    protected void Awake() {
        id = -1;
        score = 0;
        alive = true;
        maxScore = FileSystem.GetMaxScore();
        rb = GetComponent<Rigidbody2D>();
        gameHandler = FindObjectOfType<GameHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void Animate() {
        if(id == -1 && sprites.Length > 0) {
            spriteIndex++;
            if(spriteIndex == sprites.Length) spriteIndex = 0;
            spriteRenderer.sprite = sprites[spriteIndex];
        }
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
        speed.y += gravity * Time.deltaTime * 3.25f;
        transform.position += speed * Time.deltaTime / 0.7f;
    }

    protected void jump() {
        speed = Vector3.up * jumpSpeed;
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "LiteObstacle" && alive) {
            setDead();
        }
        else if(other.gameObject.tag == "Obstacle" && alive) {
            setDead();
            if(id != -1) PopulationManager.Networks[id].LiveTime -= 0.5f;
        }
        else if(other.gameObject.tag == "GroundObstacle" && alive) {
            setDead();
            if(id != -1) PopulationManager.Networks[id].LiveTime -= 1f;
        }
        else if(other.gameObject.tag == "Score") {
            addScore();
        }
    }

    // protected void OnCollisionEnter2D(Collision2D other) {
    //     if(other.gameObject.tag == "LiteObstacle" && alive) {
    //         Debug.Log("Test lite");
    //         setDead();
    //     }
    //     else if(other.gameObject.tag == "Obstacle" && alive) {
    //         Debug.Log("Test obstacle");
    //         setDead();
    //         if(id != -1) NetworkManager.Networks[id].LiveTime -= 0.5f;
    //     }
    //     else if(other.gameObject.tag == "GroundObstacle" && alive) {
    //         Debug.Log("Test ground");
    //         setDead();
    //         if(id != -1) NetworkManager.Networks[id].LiveTime -= 1f;
    //     }
    //     else if(other.gameObject.tag == "Score") {
    //         Debug.Log("Test score");
    //         addScore(1);
    //     }
    //     else {
    //         Debug.Log("xD");
    //     }
    // }

    protected void setDead() {
        alive = false;
        if(id == -1) gameHandler.GetComponent<GameHandler>().setDead();
    }

    protected void addScore() {
        if(alive) {
            score++;
        }
    }
}
