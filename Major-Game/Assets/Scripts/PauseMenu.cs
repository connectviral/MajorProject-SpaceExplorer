using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Pause_Menu;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        if (!isPaused)
        {
            Pause_Menu.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Resume()
    {
        if (isPaused)
        {
            Pause_Menu.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        isPaused = false;
    }
}
