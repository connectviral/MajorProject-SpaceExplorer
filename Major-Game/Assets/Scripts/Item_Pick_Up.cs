using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Pick_Up : MonoBehaviour
{
    private int hearts = 2;
    [SerializeField] private Text heartText;

    [SerializeField] private AudioSource CollectSoundEffect;
}
