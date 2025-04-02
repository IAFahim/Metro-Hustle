using System;
using UnityEngine;

public class GirlMove : MonoBehaviour{
    
    public float speed = 0.001f;
    
    void Update()
    {
        Vector3 position = transform.position;
        position.x += speed;
        transform.position = position;
    }
}

