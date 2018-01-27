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
        tickNoise();

        float leftRightMovement = Input.GetAxis("Horizontal");
        if (inAir)
        {
            Velocity *= 0.95f;
            Acceleration.x = leftRightMovement * airSpeed * getMobilityFactor();
        }
        else 
        {
            Velocity *= 0.8f;
            Acceleration.x = leftRightMovement * movementSpeed * getMobilityFactor();
        }

        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector2.ClampMagnitude(Velocity, 10f);


        transform.position += new Vector3(Velocity.x * Time.deltaTime, 0, 0) * getMobilityFactor();

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






    private float getMobilityFactor()
    {
        return moveResistance ? 0.7f : 1f;
    }

    private void tickNoise()
    {
        if (noisePresent) {
            if (noiseStart + 5f < Time.time) { 
                // we take damge
                Debug.Log("We took damage.");
                noiseStart = Time.time;
            }
        }
    }

    bool moveResistance = false;
    bool noisePresent = false;
    float noiseStart = 0.0f;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ResistanceVolume>() != null)
        {
            // we are entered ResistanceVolume!
            moveResistance = true;
            Debug.Log("Entered ResistanceVolume");
        }
        if (other.gameObject.GetComponent<NoiseVolume>() != null)
        {
            // we are entered ResistanceVolume!
            noisePresent = true;
            noiseStart = Time.time;
            Debug.Log("Entered NoiseVolume");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ResistanceVolume>() != null)
        {
            // we are entered ResistanceVolume!
            moveResistance = false;
            Debug.Log("Left ResistanceVolume");
        }
        if (other.gameObject.GetComponent<NoiseVolume>() != null)
        {
            // we are left NoiseVolume!
            noisePresent = false;
            noiseStart = 0;
            Debug.Log("Left NoiseVolume");
        }
    }

}
