using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public GameObject Player;
    public GameManager m_GameManager;

    void Start()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update () {
        Vector3 pos = transform.position;

        if (m_GameManager.m_Level1Cables.Length > 0){
        
            float LevelStartX = m_GameManager.m_Level1Cables[0].m_Instance.transform.position[0];
            float LevelEndX = m_GameManager.m_Level1Cables[m_GameManager.m_Level1Cables.Length - 1].m_Instance.transform.position[0];


            transform.position = new Vector3(Mathf.Clamp(Player.transform.position.x, LevelStartX, LevelEndX), pos.y, pos.z);

        }else{
            transform.position = new Vector3(Player.transform.position.x, pos.y, pos.z);
        }
	}
}
