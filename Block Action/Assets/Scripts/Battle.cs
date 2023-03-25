using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    private static Battle _b;
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
                    _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
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
    public List<Enemy> enemies;
    public List<Fighter> fighters;
    // Start is called before the first frame update
    void Start()
    {
        if (levelNumber == -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        levelNumber = PersistentDataManager.levelNumber;
        _levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
        if (levelData != null)
        {
            AudioController.audioController.PlayBGM(levelData.bgmName);
            Debug.Log(levelData.bgmName);
        }
        
        turnNumber = 1;
        //level initialization
        GimmickController.initialize();
        //for player
        bs = BattleState.Gimmicks;

        //grid initialization
        Grid.SetScale();
        GridFitter.ScaleBlocks();
        GridFitter.PlaceBlocks();
        FighterController.PlaceFighters();
    }

    // Update is called once per frame
    void Update()
    {
        if (bs.Equals(BattleState.PlayerGrid)) {
            GridFitter.GridFitting();
        //} else if (bs.Equals(BattleState.PlayerAction)) {
        //    ActionController.PlayerTurn();
        } else if (bs.Equals(BattleState.EnemyAction)) {
            ActionController.EnemyTurn();
        } else if (bs.Equals(BattleState.EnemySelect)) {
            GridFitter.EnemySelect();
        } else if (bs.Equals(BattleState.Gimmicks)) {
            GimmickController.MidLevelEffects();
        } else if (bs.Equals(BattleState.StatusEffects)) {
            Status.TriggerStatusEffects();
        }
    }

    public static bool updateDead()
    {
        if (Player.player.health <= 0)
        {
            Player.player.dead = true;
            Player.player.buff = 1;
            Player.player.defenseBuff = 1;
            Player.player.statusEffects.Clear();
            Player.player.healthBar.gameObject.SetActive(false);
            Player.player.gameObject.SetActive(false);
        }
        bool allEnemiesDead = true;
        foreach (Enemy e in b.enemies)
        {
            if (e.health <= 0)
            {
                e.dead = true;
                e.buff = 1;
                e.defenseBuff = 1;
                e.statusEffects.Clear();
                e.healthBar.gameObject.SetActive(false);
                e.gameObject.SetActive(false);
            }
            else
            {
                allEnemiesDead = false;
            }
        }
        if (Player.player.health <= 0)
        {
            BattleEndController.TriggerDefeat();
            return true;
        }
        else if (allEnemiesDead)
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
            }
            else
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    Destroy(e.healthBar);
                    Destroy(e);
                }
                Battle.b.enemies.Clear();
                BattleEndController.TriggerVictory();
                return true;
            }
        }
        return false;
    }
}
