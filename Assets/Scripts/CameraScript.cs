using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    private Camera m_MainCamera;

	// Use this for initialization
	void Start () {
        m_MainCamera = GetComponentInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 curCamPos = m_MainCamera.transform.position;
        curCamPos.y = 0;
        m_MainCamera.transform.position = curCamPos;
	}
}
