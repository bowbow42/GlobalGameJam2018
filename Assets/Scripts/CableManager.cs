using System;
using UnityEngine;

[Serializable]
public class CableManager {
    
    [HideInInspector] public Vector3 m_PadPos;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public GameObject m_Platform;
    [HideInInspector] public float m_ItemThickness;

    // Initializes Part of the cable
    public void Setup () {
    }
	
}
