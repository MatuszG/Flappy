using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgentPolicy {
    public List<float> genome;
    public int[] topology;
    public AgentPolicy(List<float> genome, int[] topology) {
        this.genome = genome;
        this.topology = topology;
    }
}
