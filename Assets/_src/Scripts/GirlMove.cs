using UnityEngine;

public class GirlMove : MonoBehaviour{
    public float speed = 0.01f;
    void Update()
    {
        var move = transform.position;
        move.x += speed;
        transform.position = move;



    }
}
