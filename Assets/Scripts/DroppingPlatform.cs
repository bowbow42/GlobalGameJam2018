using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingPlatform : MonoBehaviour {

    BoxCollider2D _coll;
    public float gravity = -5f;
    private Vector3 initPos;
    private bool wasTouched = false;
    private bool isDropping = false;
    private float velocity = 0f;
    private float secondsSinceTouch = 0f;

    void Start () {
        _coll = GetComponent<BoxCollider2D>();
        initPos = transform.position;
	}
	
	void Update () {
        if (wasTouched)
        {
            secondsSinceTouch += Time.deltaTime;
        }
        if (isDropping)
        {
            velocity += gravity * Time.deltaTime;
            transform.position += new Vector3(0, velocity);
        }
        if(secondsSinceTouch > 2f && !isDropping)
        {
            initiateDrop();
        }
        if(secondsSinceTouch > 4f)
        {
            resetPlatform();
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!wasTouched && collision.gameObject.CompareTag("Player"))
        {
            wasTouched = true;
        }
    }

    void initiateDrop()
    {
        isDropping = true;
    }

    void resetPlatform()
    {
        secondsSinceTouch = 0f;
        velocity = 0f;
        wasTouched = false;
        isDropping = false;
        transform.position = initPos;
    }
}
