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
                        ActivateMidLevelEffect(gimmickInfo, 2);
                        gimmickController.midLevelEffects.RemoveAt(i);
                        i--;
                    }
                }
                else if (gimmickInfo[0].Equals("wave"))
                {
                    int wave = System.Convert.ToInt32(gimmickInfo[1]);
                    if (wave == FighterController.fighterController.wave)
                    {
                        ActivateMidLevelEffect(gimmickInfo, 2);
                        gimmickController.midLevelEffects.RemoveAt(i);
                        i--;
                    }
                }
                else if (gimmickInfo[0].Equals("repeating"))
                {
                    ActivateMidLevelEffect(gimmickInfo, 1);
                }
            }
            Battle.b.bs = BattleState.PlayerGrid;
            return;
        }
    }

    public static void ActivateMidLevelEffect(string[] gimmickInfo, int i)
    {
        if (gimmickInfo[i].Equals("text"))
        {
            TextAsset t = (TextAsset)Resources.Load($"Levels/{gimmickInfo[i + 1]}");
            gimmickController.text = t.text;
            Debug.Log(gimmickController.text);
        }
        else if (gimmickInfo[i].Equals("damage"))
        {
            int damage = System.Convert.ToInt32(gimmickInfo[i + 2]);
            if (gimmickInfo[i + 1].Equals("player"))
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
            else if (gimmickInfo[i + 1].Equals("enemies"))
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
        else if (gimmickInfo[i].Equals("heal"))
        {
            int heal = System.Convert.ToInt32(gimmickInfo[i + 2]);
            if (gimmickInfo[i + 1].Equals("player"))
            {
                if (!Player.player.dead)
                {
                    Player.player.health += heal;
                    if (Player.player.health > Player.player.maxHealth)
                    {
                        Player.player.health = Player.player.maxHealth;
                    }
                }
            }
            else if (gimmickInfo[i + 1].Equals("enemies"))
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    if (!e.dead)
                    {
                        e.health += heal;
                        if (e.health > e.maxHealth)
                        {
                            e.health = e.maxHealth;
                        }
                    }
                }
            }
        }
        else if (gimmickInfo[i].Equals("buff"))
        {
            double buffValue = System.Convert.ToDouble(gimmickInfo[i + 2]);
            int length = System.Convert.ToInt32(gimmickInfo[i + 3]);
            if (gimmickInfo[i + 1].Equals("player"))
            {
                if (!Player.player.dead)
                {
                    BuffCounter bc = new BuffCounter(length, buffValue);
                    Player.player.buffLeft.Add(bc);
                    Player.player.buff *= buffValue;
                }
            }
            else if (gimmickInfo[i + 1].Equals("enemies"))
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    if (!e.dead)
                    {
                        BuffCounter bc = new BuffCounter(length, buffValue);
                        e.buffLeft.Add(bc);
                        e.buff *= buffValue;
                    }
                }
            }
        }
        else if (gimmickInfo[i].Equals("cooldown"))
        {
            foreach (SoulObject s in Battle.b.soulObjects)
            {
                if (!s.placed)
                {
                    s.currentCooldown += System.Convert.ToInt32(gimmickInfo[i + 1]);
                    if (s.currentCooldown < 0)
                    {
                        s.currentCooldown = 0;
                    }
                    s.changeCooldownColor();
                }
            }
        }
    }
}
