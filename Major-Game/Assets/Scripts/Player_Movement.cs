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

    [SerializeField] private AudioSource JumpSoundEffect;

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
            JumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
        }

        updateplayerAnimation();
    }

    private void updateplayerAnimation()
    {
        MovementState newMovementState = MovementState.idel;

        if (dirx > 0f || dirx < 0f)
        {
            newMovementState = MovementState.running;
            RL.flipX = dirx < 0f;
        }
        else if (rb.velocity.y > 0.1f)
        {
            newMovementState = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f && IsGrounded())
        {
            newMovementState = MovementState.vanished;
        }

        anim.SetInteger("animmovement", (int)newMovementState);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
