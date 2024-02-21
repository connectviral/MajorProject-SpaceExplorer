using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer RL;
    private float dirx = 0f;

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
        float dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * 7f, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, 14f);
        }

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

    private void updateplayerAnimation()
    {
        
    }
}
