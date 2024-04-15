// Finished.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedLev : MonoBehaviour
{
    [SerializeField] private AudioSource FinishedSoundEffect;

    private bool levelcompleted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !levelcompleted)
        {
            FinishedSoundEffect.Play();
            levelcompleted = true;
            Invoke("CompleteLevel", 2f);
        } 
    }
    private void CompleteLevel()
    {
        UnlockNewLevel();
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log("Next Level Index: " + nextLevelIndex);
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Debug.Log("No more levels available.");
        }
    }


    private void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
