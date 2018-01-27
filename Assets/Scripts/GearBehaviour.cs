using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBehaviour : MonoBehaviour {

    private bool isTurning = false;
    private float rotationVelocity = 0.8f;
    private float StartAngle = 0f;
    private float TargetAngle = 0f;

    // between 0 and 1 for lerp
    private float AngleProgress = 0f;

    private void Start()
    {
        StartAngle = transform.eulerAngles.z;
        TargetAngle = StartAngle + 90f;
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
                StartAngle += 90f;
                TargetAngle += 90f;
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
