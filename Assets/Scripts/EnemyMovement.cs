using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public bool goRight;
	
	void Update () {
        if(!goRight)
            transform.position += new Vector3(-5 * Time.deltaTime, 0, 0);
        else
            transform.position += new Vector3(5 * Time.deltaTime, 0, 0);

    }
}
