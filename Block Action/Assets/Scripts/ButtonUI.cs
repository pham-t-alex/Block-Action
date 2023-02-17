using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void MainMenu()
    {
        PersistentDataManager.storyOnly = false;
        PersistentDataManager.levelNumber = 0;
        PersistentDataManager.storyState = 0;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Go to Main Menu");
    }

    public void NewGame()
    {
        PersistentDataManager.storyOnly = false;
        PersistentDataManager.levelNumber = 0;
        PersistentDataManager.storyState = 0;
        SceneManager.LoadScene("LevelSelection");
        Debug.Log("Go to Level Selection Scene.");
    }

    public void StageSelect()
    {
        PersistentDataManager.storyOnly = false;
        PersistentDataManager.levelNumber = 0;
        PersistentDataManager.storyState = 0;
        SceneManager.LoadScene("StageSelection");
        Debug.Log("Go to Stage Selection scene");
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
