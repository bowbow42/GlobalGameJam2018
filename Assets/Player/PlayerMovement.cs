using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public int movementSpeed;
    public int jumpForce;

    private bool inAir = false;

    private Rigidbody2D _rb;
    private Collider2D _coll;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        float leftRightMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(leftRightMovement * movementSpeed * Time.deltaTime, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && !inAir)
            jump();
    }

    void jump()
    {
        _rb.AddForce(transform.up * jumpForce);
        inAir = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            inAir = false;
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            inAir = true;
    }

}
