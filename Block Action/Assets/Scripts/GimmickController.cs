using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    // Start is called before the first frame update
    private static GimmickController _gimmickController;
    public static GimmickController gimmickController
    {
        get
        {
            if (_gimmickController == null)
            {
                _gimmickController = FindObjectOfType<GimmickController>();
            }
            return _gimmickController;
        }
    }

    public List<string> midLevelEffects;
    bool effectHappening;
    string text;
    void Start()
    {
        midLevelEffects = new List<string>(Resources.Load<LevelData>($"Levels/Level {FighterController.fighterController.levelNumber}").midLevelEffects);
        effectHappening = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void MidLevelEffects()
    {
        if (!gimmickController.effectHappening)
        {
            for (int i = 0; i < gimmickController.midLevelEffects.Count; i++)
            {
                string[] gimmickInfo = gimmickController.midLevelEffects[i].Split(" ");
                if (gimmickInfo[0].Equals("Turn"))
                {
                    int turn = System.Convert.ToInt32(gimmickInfo[1]);
                    if (turn == Battle.b.turnNumber)
                    {
                        if (gimmickInfo[2].Equals("Text"))
                        {
                            TextAsset t = (TextAsset) Resources.Load($"Levels/{gimmickInfo[3]}");
                            gimmickController.text = t.text;
                            Debug.Log(gimmickController.text);
                            gimmickController.midLevelEffects.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            Battle.b.bs = BattleState.PlayerGrid;
            return;
        }
    }
}
