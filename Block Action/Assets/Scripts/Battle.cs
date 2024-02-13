using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Battle : MonoBehaviour
{
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    private static Battle _b;
    public bool newWave = false;
    public int turnNumber;
    public int _levelNumber = -1;
    public int levelNumber
    {
        get
        {
            if (_levelNumber == -1)
            {
                _levelNumber = PersistentDataManager.levelNumber;
            }
            return _levelNumber;
        }
        set
        {
            _levelNumber = value;
        }
    }
    public int wave;
    private LevelData _levelData;
    public LevelData levelData
    {
        get
        {
            if (_levelData == null)
            {
                if (levelNumber < 0)
                {
                    return null;
                }
                else
                {
                    if (PersistentDataManager.bonusVariant)
                    {
                        _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}B");
                    }
                    else
                    {
                        _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
                    }
                    if (_levelData == null)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("StageSelection");
                    }
                }
            }
            return _levelData;
        }
    }
    public static Battle b
    {
        get
        {
            if (_b == null)
            {
                _b = FindObjectOfType<Battle>();
            }
            return _b;
        }
    }
    public List<Enemy> enemies = new List<Enemy>();
    public List<Fighter> fighters = new List<Fighter>();
    // Start is called before the first frame update
    void Start()
    {
        if (levelNumber == -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        levelNumber = PersistentDataManager.levelNumber;
        if (PersistentDataManager.bonusVariant)
        {
            _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}B");
        }
        else
        {
            _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
        }
        if (levelData != null)
        {
            AudioController.audioController.PlayBGM(levelData.bgmName);
            string background = levelData.background;
            if (background == "Forest Sky")
            {
                GameObject.Find("forestbg").SetActive(false);
                GameObject.Find("darkforestparticles").SetActive(false);
                GameObject.Find("treebasebg").SetActive(false);
                GameObject.Find("treebaseparticles").SetActive(false);
            }
            else if (background == "Tree Base")
            {
                GameObject.Find("treebossbg").SetActive(false);
                GameObject.Find("skyparticles").SetActive(false);
                GameObject.Find("forestbg").SetActive(false);
                GameObject.Find("darkforestparticles").SetActive(false);
            }
            else
            {
                GameObject.Find("treebossbg").SetActive(false);
                GameObject.Find("skyparticles").SetActive(false);
                GameObject.Find("treebasebg").SetActive(false);
                GameObject.Find("treebaseparticles").SetActive(false);
            }
            Debug.Log(levelData.bgmName);
        }
        
        turnNumber = 1;
        //level initialization
        GimmickController.initialize();
        //for player
        Player.player.Initialize();

        //grid initialization
        Grid.SetScale();
        GridFitter.ScaleBlocks();
        GridFitter.PlaceBlocks();
        FighterController.PlaceFighters();

        bs = BattleState.Gimmicks;
        GimmickController.MidLevelEffects();
    }

    // Update is called once per frame
    void Update()
    {
        if (bs.Equals(BattleState.PlayerGrid)) {
            GridFitter.GridFitting();
        //} else if (bs.Equals(BattleState.PlayerAction)) {
        //    ActionController.PlayerTurn();
        //} else if (bs.Equals(BattleState.EnemyAction)) {
        //    ActionController.EnemyTurn();
        } else if (bs.Equals(BattleState.EnemySelect)) {
            GridFitter.EnemySelect();
        //} else if (bs.Equals(BattleState.Gimmicks)) {
        //    GimmickController.MidLevelEffects();
        //} else if (bs.Equals(BattleState.StatusEffects)) {
        //    Status.TriggerStatusEffects();
        }
    }

    public async static Task UpdateDead()
    {
        if (Player.player.dead && !Player.player.faded)
        {
            await Player.player.Fade();
        }
        
        foreach (Enemy e in b.enemies)
        {
            if (e.dead && !e.faded)
            {
                await e.Fade();
            }
        }
    }

    public static bool finishedDead()
    {
        if (Player.player.health <= 0)
        {
            BattleEndController.TriggerDefeat();
            return true;
        }
        bool allEnemiesDead = true;
        foreach (Enemy e in b.enemies)
        {
            if (!e.dead)
            {
                allEnemiesDead = false;
            }
        }
        if (allEnemiesDead)
        {
            if (Battle.b.wave < Battle.b.levelData.enemyWaves.Count)
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    Destroy(e.healthBar);
                    Destroy(e);
                }
                Battle.b.enemies.Clear();
                Battle.b.wave++;
                FighterController.PlaceFighters();
                /*GridFitter.ResetSoulObjects();
                Battle.b.turnNumber++;
                b.bs = BattleState.Gimmicks;
                GimmickController.MidLevelEffects();*/

                ActionUserParticle.actionUserParticle.disable();
                GridFitter.ResetSoulObjects();
                GimmickController.gimmickController.index = 0;
                Battle.b.bs = BattleState.StatusEffects;
                Debug.Log("Status Effects");
                b.newWave = true;
                Status.TriggerStatusEffects();
            }
            else
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    Destroy(e.healthBar);
                    Destroy(e);
                }
                Battle.b.enemies.Clear();
                if (!BattleEndController.battleEndController.victorious)
                {
                    BattleEndController.TriggerVictory();
                }
                else
                {
                    b.bs = BattleState.End;
                }
                
            }
            return true;
        }
        return false;
    }
}
