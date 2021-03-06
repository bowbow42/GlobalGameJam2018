﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBehaviour : MonoBehaviour {

    private bool isTurning = false;
    private float rotationVelocity = 1f;
    private float StartAngle = 0f;
    private float TargetAngle = 0f;
    public bool clockwise = false;

    // between 0 and 1 for lerp
    private float AngleProgress = 0f;

    private void Start()
    {
        StartAngle = transform.eulerAngles.z;
        TargetAngle = clockwise ? StartAngle - 90f : StartAngle + 90f;
    }

    void Update () {
		if(isTurning)
        {
            AngleProgress += Time.deltaTime * rotationVelocity;
            float deltaAngle = Mathf.LerpAngle(StartAngle, TargetAngle, AngleProgress);
            transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, deltaAngle);

            if (AngleProgress > 1f) 
            {
                isTurning = false;
                AngleProgress = 0f;
                if (clockwise)
                {
                    StartAngle -= 90f;
                    TargetAngle -= 90f;
                }
                else
                {
                    StartAngle += 90f;
                    TargetAngle += 90f;
                }
                transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, StartAngle);
            }
        }
	}

    public void Action()
    {
        if(!isTurning)
        {
            isTurning = true;
        }
    }
}
