using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Go to Main Menu");
    }

    public void NewGame()
    {
        SceneManager.LoadScene("LevelSelection");
        Debug.Log("Go to Level Selection Scene.");
    }

    public void DarkForest()
    {
        SceneManager.LoadScene("Story");
        Debug.Log("Dark Forest Level");
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit Game.");
    }

    // Might not need
    public void Save()
    {
        Debug.Log("Save Settings");
    }
}
