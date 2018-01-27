using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed;
    public float movementDampening;
    public float airSpeed;
    public float jumpForce;
    public float leaveGroundInterval;
    public float leaveGroundDistance;
    public Vector2 Gravity;
    public Vector2 MaxVelocity;

    private bool inAir = true;

    private Vector2 FloorNormal;
    private Vector2 MovementDir;

    private Vector2 Velocity = new Vector2();
    private Vector2 Acceleration = new Vector2();

    private float leaveGroundStart;
    //private Rigidbody2D _rb;
    private Collider2D _coll;
    private CircleCollider2D _circle;

    private Volume inVolume;
    private enum Volume { None, Noise, Resistance, Push, Dash, DoubleJump, Floaty};

    void Start()
    {
        //_rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();
        _circle = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate() {
        tickNoise();
        float leftRightMovement = Input.GetAxis("Horizontal");

		if (inAir)
		{
            Velocity *= 0.95f;
            Acceleration = MovementDir * leftRightMovement * airSpeed + Gravity;
			Acceleration *= getMobilityFactor();

            Velocity += Acceleration * Time.deltaTime;
            Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(Mathf.Abs(Velocity.x), MaxVelocity.x);
            transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
        }
        else 
        {
            Velocity *= 0.8f;
            Acceleration.x = leftRightMovement * movementSpeed;
			Acceleration *= getMobilityFactor();

            Velocity += Acceleration * Time.deltaTime;
            // Debug.Log(Velocity);
            
            Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(Mathf.Abs(Velocity.x), MaxVelocity.x);
            Velocity.y = Mathf.Sign(Velocity.y) * Mathf.Min(Mathf.Abs(Velocity.y), MaxVelocity.y);
            transform.position += new Vector3(MovementDir.x * Velocity.x, MovementDir.y * Velocity.x, 0) * Time.deltaTime;
                        
            if (leaveGroundStart != 0.0f)
            {
                
                if (leaveGroundStart + leaveGroundInterval < Time.time)
                {
                    inAir = true;
                    MovementDir = Vector2.right;
                }
                else
                {
                    RaycastHit2D[] contacts = new RaycastHit2D[1];
                    Debug.Log("Casting");
                    if(_coll.Cast(Vector2.down, contacts, leaveGroundDistance, true) > 0)
                    {
                        Debug.Log(contacts[0].distance);
                        if (contacts[0].collider.CompareTag("Platform"))
                        {
                            if (!(contacts[0].normal.y - 0.5 <= 0))
                                transform.position += Vector3.down * contacts[0].distance;
                        }
                    }
                }
                
            }

            
        }

        Acceleration = new Vector2();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAir)
            jump();
    }

    void jump()
    {
        Velocity.y = jumpForce;
        inAir = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log("Hit something");
        leaveGroundStart = 0.0f;
        if (collision.gameObject.CompareTag("Platform"))
        {
            Velocity.y = 0;
            FloorNormal = new Vector2();
            int count = 0;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y - 0.5 <= 0) continue;
                FloorNormal += contact.normal;
                count++;
            }
            
            if (count > 0)
            {
                MovementDir = new Vector2(FloorNormal.y, -FloorNormal.x );
                if (MovementDir.x < 0)
                    MovementDir *= -1;
                MovementDir = MovementDir / count;               
                inAir = false;
            }
            else
                MovementDir = Vector2.right;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            leaveGroundStart = Time.time;
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        switch(tag)
        {
            case "V_Noise":
                inVolume = Volume.Noise;
                break;
            case "V_Resistance":
                inVolume = Volume.Resistance;
                break;
            case "V_Push":
                inVolume = Volume.Push;
                break;
            case "V_Dash":
                inVolume = Volume.Dash;
                break;
            case "V_DoubleJump":
                inVolume = Volume.DoubleJump;
                break;
            case "Floaty":
                inVolume = Volume.Floaty;
                break;
            default:
                inVolume = Volume.None;
                break;
        }
    }
}
