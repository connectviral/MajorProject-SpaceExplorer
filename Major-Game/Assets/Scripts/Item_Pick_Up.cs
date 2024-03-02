using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Pick_Up : MonoBehaviour
{
    private int bullets = 0;
    [SerializeField] private Text bulletText;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("pick_up"))
        {
            Destroy(collision.gameObject);
            bullets++; 
            bulletText.text = ("Bullets: "+ bullets); 
        }
    }
}
