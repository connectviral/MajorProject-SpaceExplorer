using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Text playerDisplay;
    private void Start()
    {
        if(DBManager.LoggedIn)
        {
            playerDisplay.text = "Player: " + DBManager.username;
        }
    }
    public void GoToLogin()
    {
        SceneManager.LoadScene(0);
    }
    public void GoToLoginPage()
    {
        SceneManager.LoadScene(1);
    }
    public void GoToRegister()
    {
        SceneManager.LoadScene(2);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(4);
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
