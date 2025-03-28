using UnityEngine;

public class GirlMove : MonoBehaviour
{
    public float speed = 0.001f;
    void Update()
    {
        var vector3 = transform.position;
        vector3.x = vector3.x + speed;
        transform.position = vector3;
    }
}
