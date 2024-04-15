// Import necessary libraries
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    // Declare variables
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer RL;
    private float dirx = 0f;
    [SerializeField] private Transform respawnPosition;

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
        // Get components and connect to server
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        RL = GetComponent<SpriteRenderer>();

        ConnectToServer();
    }

    private void ConnectToServer()
    {
        // Connect to server
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
        // Read data from server
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

    private bool isMoving = false;

    private void HandleCommand(string command)
    {
        // Handle commands from server
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
                isMoving = true;
                dirx = 1f;
                break;
            case "left":
                isMoving = true;
                dirx = -1f;
                break;
            case "stop":
                isMoving = false;
                dirx = 0f;
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        // Read keyboard inputs for movement
        float keyboardInput = Input.GetAxisRaw("Horizontal");

        // Read network commands for movement
        float networkInput = isMoving ? dirx : 0f;

        // Combine keyboard and network inputs for movement
        dirx = Mathf.Clamp(keyboardInput + networkInput, -1f, 1f);

        if (dirx != 0)
        {
            // Player is moving, update velocity
            rb.velocity = new Vector2(dirx * Movespeed, rb.velocity.y);
        }
        else
        {
            // Player is not moving, stop the player
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

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
        // Update player animation
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
        // Check if player is grounded
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Respawn()
    {
        // Respawn player
        rb.velocity = Vector2.zero; // Reset the player's velocity
        transform.position = respawnPosition.position; // Move the player to the respawn position
    }

    private void Die()
    {
        // Implement your logic for player death
    }

    private void OnDestroy()
    {
        // Close connection
        CloseConnection();
    }

    public void CloseConnection()
    {
        // Close connection
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
