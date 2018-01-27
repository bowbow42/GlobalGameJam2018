using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformUpAndDown : MovingPlatform
{
    public override void Start()
    {
        if (start == Vector3.zero && end == Vector3.zero)
        {
            Vector3 pos = transform.position;
            start = pos - new Vector3(0, 5, 0);
            end = pos + new Vector3(0, 5, 0);
        }
        base.Start();
        speed = 2;

        // Randomized position and direction
        transform.position = Vector3.Lerp(start, end, Random.value);
        moveFromStartToEnd = Random.value > 0.5;
    }
}
