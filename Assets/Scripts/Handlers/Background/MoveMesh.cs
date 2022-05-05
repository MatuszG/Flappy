using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMesh : MonoBehaviour
{
    public float speed = 1f;
    private MeshRenderer mesh;

    private void Awake() {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.mainTextureOffset = new Vector2(1000, 0);
    }

    private void FixedUpdate() {
        mesh.material.mainTextureOffset -= new Vector2(speed * Time.deltaTime, 0);
        if(mesh.material.mainTextureOffset.x < 0) {
            mesh.material.mainTextureOffset = new Vector2(1000, 0);
        }
    }
}
