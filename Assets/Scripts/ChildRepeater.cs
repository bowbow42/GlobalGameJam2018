using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildRepeater : MonoBehaviour {

    public GameObject ObjectToRepeat;
    public int ObjectCount;
    public float Offset;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < ObjectCount; i++ )
        {
            GameObject obj = Instantiate(ObjectToRepeat, transform) as GameObject;
            obj.gameObject.transform.localPosition = new Vector3(0f, 0f, Offset * i);

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
