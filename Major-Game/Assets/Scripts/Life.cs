using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private float deathYPosition = -10f;
    public static int life = 2;

    private bool hasLifeDecremented = false;

    [SerializeField] private Text heartText;
    [SerializeField] private AudioSource DeathSoundEffect;
    [SerializeField] private AudioSource CollectSoundEffect;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        UpdateLifeText();
    }

    private void Update()
    {
        // Check if life is already 0 before calling Die()
        if (life > 0 && transform.position.y < deathYPosition && !hasLifeDecremented)
        {
            Die();
            hasLifeDecremented = true; // Set the flag to true to indicate that the decrement has occurred
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (life > 0)
        {
            DeathSoundEffect.Play();
            rb.bodyType = RigidbodyType2D.Static;
            anim.SetTrigger("death");
            life--;
            UpdateLifeText();
            Debug.Log("Life decremented. Current life: " + life);
            Invoke("RestartLevel", 2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pick_up"))
        {
            CollectSoundEffect.Play();
            Destroy(collision.gameObject);
            life++;
            UpdateLifeText();
        }
    }

    private void RestartLevel()
    {
        if (life > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    private void UpdateLifeText()
    {
        heartText.text = "❤️ " + life;
    }
}
