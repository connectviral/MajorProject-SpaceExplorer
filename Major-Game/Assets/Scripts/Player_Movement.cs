using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer RL;
    private float dirx = 0f;

    [SerializeField] private float Movespeed = 5f;
    [SerializeField] private float Jumpforce = 21f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idel, running, jumping, vanished }
    private MovementState animmovement = MovementState.idel;

    [SerializeField] private AudioSource JumpSoundEffect;

    private TcpClient client;
    private NetworkStream stream;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        RL = GetComponent<SpriteRenderer>();

        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            client = new TcpClient("localhost", 12345);
            stream = client.GetStream();
            ReadDataAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error connecting to server: {e.Message}");
        }
    }

    private async void ReadDataAsync()
    {
        try
        {
            while (true)
            {
                byte[] data = new byte[256];
                int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                if (bytesRead > 0)
                {
                    string command = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                    HandleCommand(command);
                }
            }
        }
        catch (IOException e) when (e.InnerException is SocketException se && se.ErrorCode == 10053)
        {
            Debug.Log("Connection closed by host machine");
            // Handle the connection closed event here
        }
        catch (Exception e)
        {
            Debug.Log("Error reading data: " + e.Message);
        }
    }

    private void HandleCommand(string command)
    {
        switch (command)
        {
            case "space":
                if (IsGrounded())
                {
                    JumpSoundEffect.Play();
                    rb.velocity = new Vector2(rb.velocity.x, Jumpforce);
                }
                break;
            case "right":
                dirx = 1f;
                break;
            case "left":
                dirx = -1f;
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        // Read keyboard inputs for movement
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * Movespeed, rb.velocity.y);

        // Check for jump input
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

    private void OnDestroy()
    {
        CloseConnection();
    }

    public void CloseConnection()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}