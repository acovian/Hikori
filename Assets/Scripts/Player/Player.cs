using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Physics")]

    [SerializeField] float fall = 2.5f;
    [SerializeField] float jump = 2f;
    public int moveSpeed = 23;
	[SerializeField] float climbSpeed = 10;

	private Rigidbody2D rb2D;
	Rigidbody2D rb;
    bool isMoving;
	CapsuleCollider2D myBody;
	BoxCollider2D myFeet;

	float gravityScaleAtStart;

	private bool facingRight;

	Animator myAnimator;

    private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();
	}
    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
		facingRight = true;
		myFeet = GetComponent<BoxCollider2D>();
		myBody = GetComponent<CapsuleCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.freezeRotation = true;
		gravityScaleAtStart = rb2D.gravityScale;

	}

    // Update is called once per frame
    void Update()
    {
		float horizontal = Input.GetAxis("Horizontal");
		Move();
        Jump();
		ClimbLadder();
		Flip(horizontal);

		if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
		{
			myAnimator.SetBool("idling", true);
		}
		else
		{
			myAnimator.SetBool("idling", false);
		}

	}

    public void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = transform.position.x + deltaX;
        transform.position = new Vector2(newXPos, transform.position.y);

		float horizontal = Input.GetAxis("Horizontal");


		bool horizonMovement = horizontal != 0;

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
		{
			bool movement = true;
			myAnimator.SetBool("running", movement);
			if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
			{
				movement = false;
				myAnimator.SetBool("running", movement);
			}
		}

		else if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && horizonMovement == true)
		{
			bool movement = false;
			myAnimator.SetBool("running", movement);
		}

	}

    public void Jump()
    {
		if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
				bool touchingGround = myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
				myAnimator.SetBool("jumping", touchingGround);
                if (myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")))
				{
					myAnimator.SetBool("climbing", true);
				}
			}
		}

		if (rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fall - 1) * Time.deltaTime;
			bool playerVelocity = rb.velocity.y == 0;
			myAnimator.SetBool("jumping", playerVelocity);
		}
		if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.UpArrow) && !myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")))
		{
			float thing = -7f;
			rb.velocity += Vector2.up * Physics2D.gravity.y * (jump - thing) * Time.deltaTime;


		}
    }

	private void Flip(float horizontal)
	{
		if (horizontal < 0 && facingRight)
		{
			facingRight = !facingRight;
			transform.Rotate(0f, 180f, 0f);
		}

		if (horizontal > 0 && !facingRight)
		{
			facingRight = true;
			transform.Rotate(0f, 180f, 0f);

		}

	}

    private void ClimbLadder()
	{
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
			myAnimator.SetBool("climbing", false);
			rb2D.gravityScale = gravityScaleAtStart;
			return;
		}

		float controlThrow = Input.GetAxis("Vertical");
		Vector2 climbVelocity = new Vector2(rb2D.velocity.x, controlThrow * climbSpeed);
		rb2D.velocity = climbVelocity;
		rb2D.gravityScale = 0f;

		bool playerIsClimbing = Mathf.Abs(rb2D.velocity.y) > Mathf.Epsilon;
		myAnimator.SetBool("climbing", playerIsClimbing);
	}

	private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ProcessHit();
        }

    }

    private void ProcessHit()
    {
        StartCoroutine(Wait());
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
