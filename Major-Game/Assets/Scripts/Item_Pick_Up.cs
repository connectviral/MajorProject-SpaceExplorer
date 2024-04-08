﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Pick_Up : MonoBehaviour
{
    private int hearts = 0;
    [SerializeField] private Text heartText;

    [SerializeField] private AudioSource CollectSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pick_up"))
        {
            CollectSoundEffect.Play();
            Destroy(collision.gameObject);
            hearts++; 
            heartText.text = ("❤️ "+ hearts); 
        }
    }
}
