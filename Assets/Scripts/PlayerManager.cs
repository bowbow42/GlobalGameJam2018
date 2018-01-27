using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public GameObject lastSpawn;
     

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            transform.position = lastSpawn.transform.position;
    }
}
