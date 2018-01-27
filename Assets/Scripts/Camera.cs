using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public float LevelStartX;
    public float LevelEndX;

    public GameObject Player;

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update () {
        Vector3 pos = transform.position;
        
        transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x, LevelStartX, LevelEndX), pos.y, pos.z);
	}
}
