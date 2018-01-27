using System;
using UnityEngine;

[Serializable]
public class CableManager {

    public Color m_PadColor = Color.white;
    public Vector3 m_PadPos;
    [HideInInspector] public GameObject m_Instance;
    
    // Initializes Part of the cable
    public void Setup () {
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PadColor;
        }

        Transform[] transformations = m_Instance.GetComponentsInChildren<Transform>();
        transformations[1].position = m_PadPos + transformations[0].position;
    }
	
}
