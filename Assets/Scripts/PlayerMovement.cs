using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed;
    public float movementDampening;
    public float airSpeed;
    public float jumpForce;

    private bool inAir = true;

    private Vector2 FloorNormal;

    private Vector2 MaxValocity = new Vector2(10,10);
    private Vector2 Velocity = new Vector2();
    private Vector2 Acceleration = new Vector2();

    private Rigidbody2D _rb;
    private Collider2D _coll;
    private CircleCollider2D _circle;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();
        _circle = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate() { 
        float leftRightMovement = Input.GetAxis("Horizontal");
        if (inAir)
        {
            Velocity *= 0.95f;
            Acceleration.x = leftRightMovement * airSpeed;
        }
        else 
        {
            Velocity *= 0.8f;
            Acceleration.x = leftRightMovement * movementSpeed;
        }

        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, 10f);


        transform.position += new Vector3(Velocity.x * Time.deltaTime, 0, 0);

        Acceleration.x = 0;
        Acceleration.y = 0;
    }

    void Update()
    {
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
        {
            ContactPoint2D[] contacts = collision.contacts;
            float x = 0;
            float y = 0;
            int count = 0;
            for (int i = 0; i < contacts.Length; i++)
            {
                if (contacts[i].normal.y - 0.70 <= 0) continue;
                count++;
                x += contacts[i].normal.x;
                y += contacts[i].normal.x;
            }

            if (count > 0) {
                x *= 1.0f / count;
                y *= 1.0f / count;
                FloorNormal = new Vector2(x, y);
                inAir = false;
            }

            foreach (ContactPoint2D contact in contacts)
            {

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            inAir = true;
    }

}
