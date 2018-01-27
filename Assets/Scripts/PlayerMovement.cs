using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed;
    public float movementDampening;
    public float airSpeed;
    public float jumpForce;
    public Vector2 Gravity;
    public Vector2 MaxVelocity;

    private bool inAir = true;

    private Vector2 FloorNormal;
    private Vector2 MovementDir;

    private Vector2 Velocity = new Vector2();
    private Vector2 Acceleration = new Vector2();

    //private Rigidbody2D _rb;
    private Collider2D _coll;
    private CircleCollider2D _circle;

    void Start()
    {
       // _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<BoxCollider2D>();
        _circle = GetComponent<CircleCollider2D>();
    }

    void FixedUpdate() { 
        float leftRightMovement = Input.GetAxis("Horizontal");
     if (inAir)
        {
            Velocity *= 0.95f;
            Acceleration = MovementDir * leftRightMovement * airSpeed + Gravity;

            Velocity += Acceleration * Time.deltaTime;

            if (Mathf.Abs(Velocity.x) > MaxVelocity.x)
            {
                if (Velocity.x > 0)
                    Velocity.x = MaxVelocity.x;
                else
                    Velocity.x = -MaxVelocity.x;
            }
            transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;

        }
        else 
        {
            
            Velocity *= 0.8f;
            Acceleration.x = leftRightMovement * movementSpeed;

            Velocity += Acceleration * Time.deltaTime;
            // Debug.Log(Velocity);
            Velocity = Vector2.ClampMagnitude(Velocity, MaxVelocity.x);
            transform.position += new Vector3(MovementDir.x * Velocity.x, MovementDir.y * Velocity.x, 0) * Time.deltaTime;
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
        if (collision.gameObject.CompareTag("Platform"))
        {
            Velocity.y = 0;
            FloorNormal = new Vector2();
            int count = 0;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                //if (contact.normal.y - 0.707 <= 0) continue;
                FloorNormal += contact.normal;
                count++;
            }
            
            if (count > 0)
            {
                
                
                MovementDir = new Vector2(FloorNormal.y, -FloorNormal.x );
                if (MovementDir.x < 0)
                    MovementDir *= -1;
                Debug.Log("DAvor" + MovementDir);
                MovementDir = MovementDir / count;
                Debug.Log(MovementDir);
                inAir = false;
            }
            else
                MovementDir = Vector2.right;

            

            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            inAir = true;

        MovementDir = Vector2.right;
    }
}
