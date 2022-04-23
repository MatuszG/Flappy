using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgentPolicy {
    public List<float> genome;
    public AgentPolicy(List<float> genome) {
        this.genome = genome;
    }
}
