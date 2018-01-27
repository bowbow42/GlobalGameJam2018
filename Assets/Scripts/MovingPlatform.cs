using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Vector3 start;
    public Vector3 end;
    public float speed = 1;

    private Vector3 direction;
    private float length;
    protected bool moveFromStartToEnd = true;

    public virtual void Start()
    {
        direction = end - start;
        direction.Normalize();
        length = Vector3.Distance(end, start);
        transform.position = start;

        if (start == Vector3.zero || end == Vector3.zero)
        {
            Debug.Log("Moving Platform Start or End Point is (0,0,0)");
        }
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
