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
        SceneManager.LoadScene(7);
    }
    public void GoToLoginPage()
    {
        SceneManager.LoadScene(9);
    }
    public void GoToRegister()
    {
        SceneManager.LoadScene(8);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
