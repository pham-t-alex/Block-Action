using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    List<string> sceneHistory = new List<string>();

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
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Dark Forest Level");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
        Debug.Log("Gone to Settings Scene.");
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit Game.");
    }

    //Stuck
    public void Back(string name)
    {
        Debug.Log("Back to previous Scene");
    }

    // Might not need
    public void Save()
    {
        Debug.Log("Save Settings");
    }
}
