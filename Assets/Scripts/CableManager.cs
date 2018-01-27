﻿using System;
using UnityEngine;

[Serializable]
public class CableManager {
    
    [HideInInspector] public Vector3 m_PadPos;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public GameObject m_Platform;
    
    // Initializes Part of the cable
    public void Setup () {
        //MeshRenderer renderer = m_Platform.GetComponentInChildren<MeshRenderer>();
        //renderer.material = m_Textures[0];
    }
	
}
