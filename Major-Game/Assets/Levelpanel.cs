using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelpanel : MonoBehaviour
{
    public void OpenLevel(int levelId)
    {
        int levelName = levelId;
        SceneManager.LoadScene(levelName);
    }
}
