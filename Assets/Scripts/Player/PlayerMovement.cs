using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float dirX = 0f;
    private bool flippedSprite;
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 11f;

    private Rigidbody2D rb;
    private BoxCollider2D coll;     //Check if grounded
    private SpriteRenderer sprite;  //Flip player sprite
    private Animator anim;          //Control animation

    [SerializeField] private LayerMask jumpableGround;  // REQUEST

    private enum MovementState { idle, running, jumping, falling }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();    //Flip the image when move
        anim = GetComponent<Animator>(); // Running animation
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");    // Release key will stop move
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);  // Move the player Horizontly


        if (Input.GetButtonDown("Jump") && IsGrounded())    // 'Jump' - From Input Manager > Project Setting
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);   // Move The Player Vertically 
        }

        UpdateAnimationState(); //Switch Animation

        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            Application.Quit();
        }
    }

    private void UpdateAnimationState()
    {
        MovementState stateNum;

        if (dirX > 0f)
        {
            stateNum = MovementState.running;   //stateNum = 1
            sprite.flipX = false;
            flippedSprite = false;
            //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (dirX < 0f)
        {
            stateNum = MovementState.running;
            sprite.flipX = true;
            flippedSprite = true;
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            stateNum = MovementState.idle;      //stateNum = 0
        }

        //For jumping animation - check y velocity
        if (rb.velocity.y > .1f && !IsGrounded())
        {
            stateNum = MovementState.jumping;   //stateNum = 2
        }
        else if (rb.velocity.y < -.1f && !IsGrounded())   // check if we falling
        {
            stateNum = MovementState.falling;       //stateNum = 3
        }

        anim.SetInteger("state", (int)stateNum);
    }

    //Double jump fix - detect collision
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    public bool canAttack()     //PLAYER CAN ATTACK IF STOP MOVING
    {
        return dirX == 0 && IsGrounded();
    }

    public bool FlipedStatus()
    {
        if (flippedSprite == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BoostPlayer(float _speedBoost, float _jumpBoost)
    {
        moveSpeed = _speedBoost;
        jumpForce = _jumpBoost;
    }
}
