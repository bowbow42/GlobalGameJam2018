using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector3 start;
    public Vector3 end;
    public float speed;

    private Vector3 direction;
    private float length;
    private bool moveFromStartToEnd = true;

    void Start()
    {
        direction = end - start;
        direction.Normalize();
        length = Vector3.Distance(end, start);
        transform.position = start;
    }

    void Update ()
    {
	    if (moveFromStartToEnd)
        {
            transform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, start) > length) 
            {
                moveFromStartToEnd = false;
            }
        }
        else
        {
            transform.position -= direction * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, end) > length)
            {
                moveFromStartToEnd = true;
            }
        }
    }
}
