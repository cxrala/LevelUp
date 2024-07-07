using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject cameraTarget;
    public float moveSpeed = 5f;

    void Update() {
        MoveCamera();
    }

    void MoveCamera() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
    
        Vector3 move = moveSpeed * Time.deltaTime * new Vector3(h, v, 0);
        cameraTarget.transform.position += move;
        //need to fix camera target moving past boundary 
    }
}
