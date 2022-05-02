using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMesh : MonoBehaviour
{
    public float speed = 1f;
    private MeshRenderer mesh;

    private void Awake() {
        mesh = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate() {
        mesh.material.mainTextureOffset -= new Vector2(speed * Time.deltaTime, 0);
    }
}
