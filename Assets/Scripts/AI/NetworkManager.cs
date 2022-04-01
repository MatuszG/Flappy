using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkManager
{
    private static NeuralNetwork[] networks;
    private static int networksN;
    private static int evolutionNumber = 0;
    public static int EvolutionNumber {
        get { return evolutionNumber; }
        set { evolutionNumber = value; }
    }
    public static NeuralNetwork[] Networks {
        get{return networks;}
        set{networks = value;}
    }
    public static int NetworksN {
        get{return networksN;}
        set{networksN = value;}
    }

    public static void create() {
        if(networksN >= 0) networks = new NeuralNetwork[networksN];
    }
}
