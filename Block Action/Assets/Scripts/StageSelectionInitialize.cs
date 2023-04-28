using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionInitialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentDataManager.levelsCompleted == -1)
        {  
            PersistentDataManager.levelNumber = 0;
            PersistentDataManager.storyState = 1;
            SceneManager.LoadScene("Story");
            Debug.Log("Dark Forest Level");
            Debug.Log("Stage: Prologue");
        }
        else if (true) //later change to if world == dark forest
        {
            AudioController.audioController.PlayBGM("darkforesttheme");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
