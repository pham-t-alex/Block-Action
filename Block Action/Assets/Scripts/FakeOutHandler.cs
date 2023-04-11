using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeOutHandler : MonoBehaviour
{
    private static FakeOutHandler _fakeOutHandler;
    public static FakeOutHandler fakeOutHandler
    {
        get
        {
            if (_fakeOutHandler == null)
            {
                _fakeOutHandler = FindObjectOfType<FakeOutHandler>();
            }
            return _fakeOutHandler;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentDataManager.levelNumber == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    bool itemUsed = false;
    bool ran = false;
    bool attacking = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (Battle.b.bs == BattleState.PlayerGrid)
        {
            ScreenDarkener.DarkenScreen();
            TextAsset textFile = Resources.Load<TextAsset>("Dialogue/Level0Attack");
            if (textFile != null)
            {
                // Use textFileName to run the cutscene
                MidlevelDialogueHandler.GetInstance().EnterDialogueMode(textFile);
            }
            attacking = true;
        }
    }

    public void Defend()
    {
        if (Battle.b.bs == BattleState.PlayerGrid)
        {
            ScreenDarkener.DarkenScreen();
            TextAsset textFile = Resources.Load<TextAsset>("Dialogue/Level0Defend");
            if (textFile != null)
            {
                // Use textFileName to run the cutscene
                MidlevelDialogueHandler.GetInstance().EnterDialogueMode(textFile);
            }
        }
    }

    public void Item()
    {
        if (Battle.b.bs == BattleState.PlayerGrid)
        {
            if (!itemUsed)
            {
                ScreenDarkener.DarkenScreen();
                TextAsset textFile = Resources.Load<TextAsset>("Dialogue/Level0Item");
                if (textFile != null)
                {
                    // Use textFileName to run the cutscene
                    MidlevelDialogueHandler.GetInstance().EnterDialogueMode(textFile);
                }
                itemUsed = true;
                attacking = true;
            }
            else
            {
                GameText.setText("No Items");
            }
        }
    }

    public void Run()
    {
        if (Battle.b.bs == BattleState.PlayerGrid)
        {
            ScreenDarkener.DarkenScreen();
            TextAsset textFile = Resources.Load<TextAsset>("Dialogue/Level0Run");

            ran = true;
            if (textFile != null)
            {
                // Use textFileName to run the cutscene
                MidlevelDialogueHandler.GetInstance().EnterDialogueMode(textFile);
            }
        }
    }

    public static void Unpause()
    {
        ScreenDarkener.UndarkenScreen();
        if (fakeOutHandler.ran)
        {
            End();
        }
        else if (fakeOutHandler.attacking)
        {
            Battle.b.bs = BattleState.PlayerAction;
            ActionController.PlayerTurn();
            fakeOutHandler.attacking = false;
        }
        else
        {
            Battle.b.bs = BattleState.EnemyAction;
            ActionController.EnemyTurn();
        }
    }

    public static void End()
    {
        BattleEndController.battleEndController.victorious = true;
        Battle.b.bs = BattleState.End;
        if (Battle.b.levelNumber == PersistentDataManager.levelsCompleted + 1)
        {
            PersistentDataManager.levelsCompleted++;
            foreach (string reward in Battle.b.levelData.firstClearRewards)
            {
                PersistentDataManager.playerBlockInventory.Add(reward);
            }
        }
        PersistentDataManager.storyState = 2;
        SceneManager.LoadScene("Story");
    }

}
