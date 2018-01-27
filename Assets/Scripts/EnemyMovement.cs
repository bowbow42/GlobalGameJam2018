using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float speed = -5f;

    private void Start()
    {
        Destroy(gameObject, 30f);
    }

    void Update () {
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
}
