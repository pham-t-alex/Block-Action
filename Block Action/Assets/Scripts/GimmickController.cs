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
                if (gimmickInfo[0].Equals("turn"))
                {
                    int turn = System.Convert.ToInt32(gimmickInfo[1]);
                    if (turn == Battle.b.turnNumber)
                    {
                        ActivateMidLevelEffect(gimmickInfo);
                        gimmickController.midLevelEffects.RemoveAt(i);
                        i--;
                    }
                }
                else if (gimmickInfo[0].Equals("wave"))
                {
                    int wave = System.Convert.ToInt32(gimmickInfo[1]);
                    if (wave == FighterController.fighterController.wave)
                    {
                        ActivateMidLevelEffect(gimmickInfo);
                        gimmickController.midLevelEffects.RemoveAt(i);
                        i--;
                    }
                }
            }
            Battle.b.bs = BattleState.PlayerGrid;
            return;
        }
    }

    public static void ActivateMidLevelEffect(string[] gimmickInfo)
    {
        if (gimmickInfo[2].Equals("text"))
        {
            TextAsset t = (TextAsset)Resources.Load($"Levels/{gimmickInfo[3]}");
            gimmickController.text = t.text;
            Debug.Log(gimmickController.text);
        }
        else if (gimmickInfo[2].Equals("damage"))
        {
            int damage = System.Convert.ToInt32(gimmickInfo[4]);
            if (gimmickInfo[3].Equals("player"))
            {
                if (!Player.player.dead)
                {
                    Player.player.health -= damage;
                    if (Player.player.health <= 0)
                    {
                        Player.player.dead = true;
                        Player.player.buff = 1;
                        Player.player.buffLeft.Clear();
                        Player.player.healthBar.gameObject.SetActive(false);
                        Player.player.gameObject.SetActive(false);
                    }
                }
            }
            else if (gimmickInfo[3].Equals("enemies"))
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    if (!e.dead)
                    {
                        e.health -= damage;
                        if (e.health <= 0)
                        {
                            e.dead = true;
                            e.buff = 1;
                            e.buffLeft.Clear();
                            e.healthBar.gameObject.SetActive(false);
                            e.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
