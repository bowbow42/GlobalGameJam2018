using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //Normal acceleration
    public float movementSpeed;
    //Movement Dampeners
    public float movementDampening;
    public float airSpeed;
    //Jump Height
    public float jumpForce;
    //Rebound when running into Walls and Ceilings
    public float reboundFactor = 0.5f;
    //public Dash vars
    public float dashForce = 20;
    public float dashTimeOut = 1;
    public int dashCounterMax = 3;
    //Character Stickyness to ground
    public float leaveGroundInterval;
    public float leaveGroundDistance;
    //Constraints
    public Vector2 Gravity;
    public Vector2 MaxVelocity;

    //MultiJump
    private bool inAir = true;
    private bool multiJump = false;
    public int multiJumpCounterMax = 1;
    private int jumpCounter = 0;

    //Dash
    public float dashTimer = 0.45f;
    public float dashFloating = 0.0f;
    private float dashStart = 0.0f;
    private float dashTimeOutTimer = 0.0f;
    private int dashCounter = 0; 
    private bool dashing = false;
    private Vector2 saveVelocity;

    //Movement Direction
    private Vector2 FloorNormal;
    private Vector2 MovementDir;

    //Speed
    private Vector2 Velocity = new Vector2();
    private Vector2 Acceleration = new Vector2();

    private float leaveGroundStart;

    //Slide of Walls
    private int beginSlideCounter = 3;
    private int slideCounter = 0;
    private bool sliding = false;

    private Collider2D _coll;
    private CircleCollider2D _circle;

    //Area Enum
    private Volume inVolume;
    private enum Volume { None, Noise, Resistance, Push, Dash, DoubleJump, Floaty};

    void Start()
    {
        _coll = GetComponent<BoxCollider2D>();
        _circle = GetComponent<CircleCollider2D>();
    }

    public void reset(){
        Velocity.x = 0f;
        Velocity.y = 0f;

        Acceleration.x = 0f;
        Acceleration.y = 0f;

        sliding = false;
        dashing = false;
        dashCounter = 0;
        dashStart = 0f;

        inAir = true;
        multiJump = false;

    }

    void FixedUpdate() {
        tickNoise();
        float leftRightMovement = Input.GetAxis("Horizontal");

        if (dashing)
        {
            //Collision prediction
            Vector2 dashVector = saveVelocity * dashForce * Time.deltaTime;
            RaycastHit2D[] hits = new RaycastHit2D[4];
            if (_coll.Cast(dashVector.normalized, hits, dashVector.magnitude, true) > 0)
            {
                foreach(RaycastHit2D hit in hits)
                {
                    if(hit.collider.CompareTag("Platform"))
                    {
                        dashVector = dashVector.normalized * hit.distance;
                        break;
                    }
                }
            }
            //actual dashing
            transform.position += new Vector3(dashVector.x, dashVector.y);
            
            if (dashStart + dashTimer > Time.time)
                return;
            if(inAir)
                Velocity.y = dashFloating;
            dashing = false;
            dashTimeOutTimer = Time.time;         
        }
        
        //Movement in Air
        if (inAir)
		{
            Acceleration = MovementDir * leftRightMovement * airSpeed + Gravity;
			Acceleration *= getMobilityFactor();

            Velocity += Acceleration * Time.deltaTime;
            Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(Mathf.Abs(Velocity.x), MaxVelocity.x);
            transform.position += new Vector3(Velocity.x, Velocity.y, 0) * Time.deltaTime;
        }
        //and on Ground
        else 
        {
            Velocity *= 0.8f;
            if (sliding)
                Acceleration = MovementDir * movementSpeed + Gravity;
            else
                Acceleration.x = leftRightMovement * movementSpeed;
			Acceleration *= getMobilityFactor();

            Velocity += Acceleration * Time.deltaTime;
            
            Velocity.x = Mathf.Sign(Velocity.x) * Mathf.Min(Mathf.Abs(Velocity.x), MaxVelocity.x);
            Velocity.y = Mathf.Sign(Velocity.y) * Mathf.Min(Mathf.Abs(Velocity.y), MaxVelocity.y);
            transform.position += new Vector3(MovementDir.x * Velocity.x, MovementDir.y * Velocity.x, 0) * Time.deltaTime;
            
            //Stick to Ground Detection
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
                    if(_coll.Cast(Vector2.down, contacts, leaveGroundDistance, true) > 0)
                    {
                        if (contacts[0].collider.CompareTag("Platform"))
                        {
                            if (!(contacts[0].normal.y - 0.5 <= 0))
                                transform.position += Vector3.down * contacts[0].distance;
                        }
                    }
                }
                
            }

            
        }

        //Dash Reset
        if (dashTimeOutTimer > 0.0f && dashTimeOutTimer + dashTimeOut < Time.time)
        {
            dashTimeOutTimer = 0.0f;
            dashCounter = 0;
        }

        Acceleration = new Vector2();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!inAir)
                jump();
            else
            {
                if (jumpCounter < multiJumpCounterMax)
                {
                    ++jumpCounter;
                    jump();
                }
            }
        }
        if(Input.GetButtonDown("Dash"))
        {
                dash();           
        }
    }

    void dash()
    {
        if (!dashing && dashCounter < dashCounterMax)
        {
            dashCounter++;
            dashStart = Time.time;
            dashing = true;
            saveVelocity = Mathf.Sign(Input.GetAxis("Horizontal")) * MovementDir;

            //If dashing opposite to movement direction, stop after dash
            if (Mathf.Sign(saveVelocity.x) != Mathf.Sign(Velocity.x))
            {
                Velocity.x = 0;
                saveVelocity = Vector2.right * Mathf.Sign(Input.GetAxis("Horizontal"));
            }
        }
    }

    void jump()
    {
        Velocity.y = jumpForce;
        inAir = true;
        MovementDir = Vector2.right;
        slideCounter = 0;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Platform"))
        //{
			
		Vector2 CollisionNormal = new Vector2();
		FloorNormal = new Vector2();
		int count = 0;
		foreach (ContactPoint2D contact in collision.contacts)
		{
			CollisionNormal += contact.normal;
			if (contact.normal.y - 0.5 <= 0) continue;
			FloorNormal += contact.normal;
			count++;
		}

        //We hit some ground
		if (count > 0)
		{
			leaveGroundStart = 0.0f;
			jumpCounter = 0;
			Velocity.y = 0;

			MovementDir = new Vector2(FloorNormal.y, -FloorNormal.x);
			if (MovementDir.x < 0)
				MovementDir *= -1;
			MovementDir = MovementDir / count;
			inAir = false;
			slideCounter = 0;
			sliding = false;
		}
        //hit wall or something
		else
		{
			if ((slideCounter > beginSlideCounter))
			{
				leaveGroundStart = 0.0f;
				jumpCounter = 0;

				sliding = true;
				MovementDir = new Vector2(CollisionNormal.y, -CollisionNormal.x);
				if (MovementDir.y > 0)
					MovementDir *= -1;
				inAir = false;
				transform.position += new Vector3(CollisionNormal.x, CollisionNormal.y) * Time.deltaTime;
				return;
			}
			slideCounter++;
			MovementDir = Vector2.right;
			Velocity = Vector2.Reflect(Velocity, CollisionNormal.normalized) * reboundFactor;
			inAir = true;              
		}   
        //}
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
                //Debug.Log("We took damage.");
                noiseStart = Time.time;
            }
        }
    }

    bool moveResistance = false;
    bool noisePresent = false;
    float noiseStart = 0.0f;

    //Collision with Status Area
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
