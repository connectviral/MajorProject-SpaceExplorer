using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer RL;
    private float dirx = 0f;
    [SerializeField] private float Movespeed = 7f;
    [SerializeField] private float Jumpforce = 18f;
    [SerializeField] private LayerMask jumpableGround;
    private enum MovementState { idel,running, jumping, vanished }
    private MovementState animmovement = MovementState.idel;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        RL = GetComponent<SpriteRenderer>();
    }
      
    // Update is called once per frame
    private void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * Movespeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
        }

        updateplayerAnimation();
    }

    private void updateplayerAnimation()
    {
        MovementState animmovement;
        if (dirx > 0f)
        {
            animmovement = MovementState.running;
            RL.flipX = false;
        }
        else if (dirx < 0f)
        {
            animmovement = MovementState.running;
            RL.flipX = true;
        }
        else
        {
            animmovement = MovementState.idel;
        }
        if (rb.velocity.y > .1f)
        {
            animmovement = MovementState.jumping;
        }
        else if(rb.velocity.y < -0.1f)
        {
            animmovement = MovementState.vanished;
        }

        anim.SetInteger("animmovement", (int)animmovement);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
