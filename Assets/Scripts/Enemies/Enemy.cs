using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speedGround;
    public float distanceGround;

    public float distanceWall;

    private bool facingRight = true;

    public Transform groundDetection;
    public Transform wallDetection;

    BoxCollider2D antBody;
	private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        antBody = GetComponent<BoxCollider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.freezeRotation = true;
	}

    // Update is called once per frame
    void Update()
    {
        Flip();
		if (antBody.IsTouchingLayers(LayerMask.GetMask("Confiner")))
		{
			Debug.Log("touching Confiner");
		}

	}

	private void Flip()
    {
        if (antBody.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            transform.Translate(Vector2.right * speedGround * Time.deltaTime);
            int playerMask = ~(LayerMask.GetMask("Player"));

			RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceGround);

            RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, distanceWall, playerMask);


            if (groundInfo.collider == false || wallInfo.collider == true)
            {
                transform.Rotate(0f, -180f, 0f);
                facingRight = false;
            }
        }
    }

}
