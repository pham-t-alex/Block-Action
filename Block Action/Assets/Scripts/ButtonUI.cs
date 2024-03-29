using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    public void MainMenu()
    {
        PersistentDataManager.storyOnly = false;
        PersistentDataManager.levelNumber = -1;
        PersistentDataManager.storyState = 0;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Go to Main Menu");
    }

    public void NewGame()
    {
        PersistentDataManager.storyOnly = false;
        if (PersistentDataManager.levelsCompleted == -1)
        {
            PersistentDataManager.levelNumber = 0;
            PersistentDataManager.storyState = 1;
            SceneManager.LoadScene("Story");
            Debug.Log("Dark Forest Level");
            Debug.Log("Stage: Prologue");
        }
        else
        {
            PersistentDataManager.levelNumber = -1;
            PersistentDataManager.storyState = 0;
            SceneManager.LoadScene("LevelSelection");
            Debug.Log("Go to Level Selection Scene.");
        }
    }

    public void StageSelect()
    {
        PersistentDataManager.storyOnly = false;
        PersistentDataManager.levelNumber = -1;
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

    public void DeveloperMode()
    {
        PersistentDataManager.levelsCompleted = 999999;
        for (int i = 1; i < 100; i++)
        {
            LevelData levelData = Resources.Load<LevelData>($"Levels/Level {i}");
            if (levelData != null)
            {
                foreach (string reward in levelData.firstClearRewards)
                {
                    PersistentDataManager.playerBlockInventory.Add(reward);
                }
            }
        }
    }
}
