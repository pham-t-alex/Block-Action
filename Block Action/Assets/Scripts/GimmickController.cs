using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
    public bool effectPause = false;
    public int index = 0;

    public static void initialize()
    {
        if (Battle.b.levelData != null)
        {
            gimmickController.midLevelEffects = new List<string>(Battle.b.levelData.midLevelEffects);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async static void MidLevelEffects()
    {
        while (gimmickController.index < gimmickController.midLevelEffects.Count)
        {
            string[] gimmickInfo = gimmickController.midLevelEffects[gimmickController.index].Split(" ");
            if (gimmickInfo[0].Equals("turn"))
            {
                int turn = System.Convert.ToInt32(gimmickInfo[1]);
                if (turn == Battle.b.turnNumber)
                {
                    await ActivateMidLevelEffect(gimmickInfo, 2);
                    gimmickController.midLevelEffects.RemoveAt(gimmickController.index);
                    gimmickController.index--;
                }
            }
            else if (gimmickInfo[0].Equals("wave"))
            {
                int wave = System.Convert.ToInt32(gimmickInfo[1]);
                if (wave == Battle.b.wave)
                {
                    await ActivateMidLevelEffect(gimmickInfo, 2);
                    gimmickController.midLevelEffects.RemoveAt(gimmickController.index);
                    gimmickController.index--;
                }
            }
            else if (gimmickInfo[0].Equals("repeating"))
            {
                await ActivateMidLevelEffect(gimmickInfo, 1);
            }
            if (Battle.b.bs == BattleState.End)
            {
                return;
            }
            if (!Battle.finishedDead())
            {
                gimmickController.index++;
            }
            else
            {
                gimmickController.index = 0;
                return;
            }
        }
        gimmickController.index = 0;
        Battle.b.bs = BattleState.PlayerGrid;
        BottomDarkener.UndarkenBottom();
        Debug.Log("Grid Fitting");
    }
    
    public async static Task ActivateMidLevelEffect(string[] gimmickInfo, int i)
    {
        if (gimmickInfo[i].Equals("dialogue"))
        {
            gimmickController.effectPause = true;
            ScreenDarkener.DarkenScreen();
            string textFileName = gimmickInfo[i + 1];
            TextAsset textFile = Resources.Load<TextAsset>("Dialogue/" + textFileName);
            if (textFile != null)
            {
                // Use textFileName to run the cutscene
                MidlevelDialogueHandler.GetInstance().EnterDialogueMode(textFile);
                while (gimmickController.effectPause == true)
                {
                    await Task.Yield();
                }
            }
            else 
            {
                Debug.Log("<color=#FF0000>Text file not found</color>");
                GimmickController.UnpauseEffects();
            }
            // When the cutscene is over, call GimmickController.UnpauseEffects()
            // GimmickController.UnpauseEffects() is called in MidLevelDialogueHandler once the dialogue is finished
        }
        else if (gimmickInfo[i].Equals("text"))
        {
            string text = "";
            for (int j = i + 1; j < gimmickInfo.Length - 1; j++)
            {
                text += gimmickInfo[j] + " ";
            }
            text += gimmickInfo[gimmickInfo.Length - 1];
            GameText.setText(text);
        }
        else if (gimmickInfo[i].Equals("damage"))
        {
            int damage = System.Convert.ToInt32(gimmickInfo[i + 2]);
            if (gimmickInfo[i + 1].Equals("player"))
            {
                if (!Player.player.dead)
                {
                    Player.player.health -= damage;
                }
            }
            else if (gimmickInfo[i + 1].Equals("enemies"))
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    if (!e.dead)
                    {
                        e.health -= damage;
                    }
                }
            }
            await Battle.UpdateDead();
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
            double buffValue = System.Convert.ToDouble(gimmickInfo[i + 3]);
            int length = System.Convert.ToInt32(gimmickInfo[i + 4]);
            if (gimmickInfo[i + 1].Equals("player"))
            {
                if (!Player.player.dead)
                {
                    if (gimmickInfo[i + 2].Equals("atk"))
                    {
                        AtkBuffStatus status = new AtkBuffStatus(length, buffValue, Player.player);
                        Player.player.statusEffects.Add(status);
                        Player.player.buff += buffValue;
                    }
                    else if (gimmickInfo[i + 2].Equals("def"))
                    {
                        DefBuffStatus status = new DefBuffStatus(length, buffValue, Player.player);
                        Player.player.statusEffects.Add(status);
                        Player.player.defenseBuff *= buffValue;
                    }
                }
            }
            else if (gimmickInfo[i + 1].Equals("enemies"))
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    if (!e.dead)
                    {
                        if (gimmickInfo[i + 2].Equals("atk"))
                        {
                            AtkBuffStatus status = new AtkBuffStatus(length, buffValue, e);
                            e.statusEffects.Add(status);
                            e.buff += buffValue;
                        }
                        else if (gimmickInfo[i + 2].Equals("def"))
                        {
                            DefBuffStatus status = new DefBuffStatus(length, buffValue, e);
                            e.statusEffects.Add(status);
                            e.defenseBuff -= buffValue;
                        }
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
        else if (gimmickInfo[i].Equals("add_block"))
        {
            SoulObject s = BlockGenerator.generateSoulObject(Resources.Load<SoulObjectData>("BlockData/" + gimmickInfo[i + 1]));
            Battle.b.soulObjects.Add(s);
            s.transform.localScale = new Vector3(GridFitter.gridFitter.scale, GridFitter.gridFitter.scale, 1);
            s.relX *= GridFitter.gridFitter.scale;
            s.relY *= GridFitter.gridFitter.scale;
            s.currentCooldown = System.Convert.ToInt32(gimmickInfo[i + 2]);
            if (s.currentCooldown < 0)
            {
                s.currentCooldown = 0;
            }
            s.changeCooldownColor();
            GridFitter.PlaceBlocks();
        }
        else if (gimmickInfo[i].Equals("remove_block"))
        {
            string name = gimmickInfo[i + 1];
            SoulObject s = null;
            for (int j = 0; j < Battle.b.soulObjects.Count; j++)
            {
                if (Battle.b.soulObjects[j].soulName == name)
                {
                    s = Battle.b.soulObjects[j];
                    Battle.b.soulObjects.RemoveAt(j);
                    break;
                }
            }
            Destroy(s.gameObject);
            GridFitter.PlaceBlocks();
        }
        else if (gimmickInfo[i].Equals("lock_tile"))
        {
            if (gimmickInfo[i + 1].Equals("random"))
            {
                Grid g = GridFitter.gridFitter.grid;
                int index = Random.Range(0, g.tiles.Count - 1);
                g.tiles[index].GetComponent<Tile>().lockTile(System.Convert.ToInt32(gimmickInfo[i + 2]));
            }
        }
        else if (gimmickInfo[i].Equals("end_level"))
        {
            BattleEndController.TriggerEnd();
        }
    }

    public static void UnpauseEffects()
    {
        gimmickController.effectPause = false;
        ScreenDarkener.UndarkenScreen();
    }
}
