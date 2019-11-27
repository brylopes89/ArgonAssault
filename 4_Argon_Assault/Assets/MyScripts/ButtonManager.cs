using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void NewGameButton(string newGameLevel)
    {
        SceneManager.LoadScene(newGameLevel);
    }
    public void ExitGameButton(string newGameLevel)
    {
        Application.Quit();
    }
}
