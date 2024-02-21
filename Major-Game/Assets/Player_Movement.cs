using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer RL;
    private float dirx = 0f;
    private float Movespeed = 7f;
    private float Jumpforce = 18f;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        RL = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * Movespeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
        }

        updateplayerAnimation();
    }

    private void updateplayerAnimation()
    {
        if (dirx > 0f)
        {
            anim.SetBool("running", true);
            RL.flipX = false;
        }
        else if (dirx < 0f)
        {
            anim.SetBool("running", true);
            RL.flipX = true;
        }
        else
        {
            anim.SetBool("running", false);
        }
        
    }
}
