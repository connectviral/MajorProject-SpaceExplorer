using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void GoToLogin()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToRegister()
    {
        SceneManager.LoadScene(2);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

    public void scene0()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
