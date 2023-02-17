using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class StageStatus : MonoBehaviour
{
    [SerializeField] public bool isStoryOnly;

    public int stageNumber;
    public bool isStageUnlocked;
    public List<GameObject> stageObjects = new List<GameObject>();

    private Color unlockedColor;
    private Color lockedColor;

    private static StageStatus _stageStatus;

    private void Awake()
    {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("LevelNode")) { 
            stageObjects.Add(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isStageUnlocked = false;
        unlockedColor = gameObject.GetComponent<Image>().color;
        lockedColor = Color.gray;

        updateStageStatus();
        changeLockedColor(); // changeLockedIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static StageStatus stageStatus
    {
        get
        {
            if (_stageStatus == null)
            {
                _stageStatus = FindObjectOfType<StageStatus>();
            }
            return _stageStatus;
        }
    }

    // Checks number of levels completed and will unlock if conditions are met
    public void updateStageStatus() {
        if (PersistentDataManager.levelsCompleted + 1 >= stageNumber) {
            isStageUnlocked = true;
            changeLockedColor(); // changeLockedIcon();
        }
    }

    // Changes the locked/unlocked appearance of the level
    public void changeLockedColor() /* changeLockedColor() */ {
        if (isStageUnlocked)
        {
            gameObject.GetComponent<Image>().color = unlockedColor;
            gameObject.GetComponent<Button>().interactable = true;
        }
        else { 
            gameObject.GetComponent<Image>().color = lockedColor;
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    // OnClick function that will set what level will be played
    public void SetCurrentLevel() {
        // PersistentDataManager.storyState = 1;
        // PersistentDataManager.levelNumber = 
        // PersistentDataManager.storyOnly =

        Debug.Log("Stage Number: " + stageNumber);
        Debug.Log("Story Only: " + isStoryOnly);
    }
}
