using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBirdHandler : BirdHandler {
    private MovePipe[] pipes;
    private float liveTime = 0;
    private float[] input = new float[PopulationManager.Topology[0]];
    private bool active = false;
    private Vector3 defaultPos = new Vector3(0, 7f, 0);
    private NeuralNetwork network;
    private GameObject notificationManager;

    public GameObject NotificationManager {
        get { return notificationManager; }
        set { notificationManager = value; }
    }

    public NeuralNetwork Network {
        get { return network; }
        set { network = value; }
    }

    public void saveAgentPolicy() {
        FileSystem.SaveAgentPolicy(network.getGenome());
    }

    public void getAgentPolicy() {
        if(FileSystem.GetAgentPolicy().Count == 0) {
            notificationManager.GetComponent<NotificationScript>().ShowNotification("Agent must be trained to perform this task! Current agent is now randomly created!");
        }
        network = new NeuralNetwork(FileSystem.GetAgentPolicy());
    }

    public void updatedNetworkTime() {
        network.LiveTime = liveTime;
    }

    public void updateNetworkScore(int score) {
        network.Score = score;
    }

    public void setPosition(Vector3 pos) {
        transform.position = pos;
    }

    public void setOn(bool active) {
        this.active = active;
    }

    public void setActive(bool active) {
        gameObject.SetActive(active);
    }

    public void setBestPipes(MovePipe[] bestPipes) {
        pipes = bestPipes;
    }

    public void restart(int i) {
        id = i;
        score = 0;
        liveTime = 0f;
        alive = true;
        active = true;
        gameObject.SetActive(true);
        transform.position = defaultPos;
        maxScore = FileSystem.GetAgentMaxScore();
        Network = PopulationManager.Networks[i];
    }

    private void Update() { 
        return;
    }

    private void FixedUpdate() { 
        if(!active) return;
        if(id == -1) pipes = PipesController.getPipes();
        if(pipes == null) return;
        input[0] = Map(transform.position.y, -0.5f, 0.75f, 0 , 1f);
        input[1] = Map(speed.y, -40f, 10f, -4f, 1f);
        input[2] = Map(pipes[0].transform.position.x, 0, 20f, 0, 1f);
        input[3] = Map(pipes[0].transform.position.y, -6f, 6f, -1f, 1f);
        // input[4] = Map(pipes[1].transform.position.x, 0, 20f, 0, 1f);
        // input[5] = Map(pipes[1].transform.position.y, -6f, 6f, -1f, 1f);
        if(network != null && network.propagate(input) > 0.5) {
            jump();
        }
        liveTime += Time.deltaTime;
        speed.y += gravity * 3.25f * Time.deltaTime;
        rb.MovePosition(transform.position + speed * Time.deltaTime / 0.7f);
    }

    private float Map(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}

