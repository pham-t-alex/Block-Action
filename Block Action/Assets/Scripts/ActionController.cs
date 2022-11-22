using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ActionController : MonoBehaviour
{
    //references player
    static PlayerAnimator playerAnimator;
    public GameObject player;
    static int soulObjectCount;

    private static ActionController _actionController;
    
    public static ActionController actionController
    {
        get
        {
            if (_actionController == null)
            {
                _actionController = FindObjectOfType<ActionController>();
            }
            return _actionController;
        }
    }

    // Awake is called before Start
    void Awake()
    {
        //References player
        playerAnimator = player.GetComponent<PlayerAnimator>();
        soulObjectCount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayerTurn()
    {
        //take the list of soul blocks placed in the grid
        foreach (SoulObject soulObject in Battle.b.placedSoulObjects)
        {
            //attack animation
            soulObjectCount++;
        }
        //if no blocks were placed in the grid, Attack animation does not play
        if (soulObjectCount > 0)
        {
            SoulObjectAnimation();
        }
        else
        {
            Battle.b.bs = BattleState.EnemyAction;
            Debug.Log("Enemy Turn");
        }
    }

    public async static void SoulObjectAnimation()
    {
        if (soulObjectCount > 0)
        {
            for (int obj = 0; obj < soulObjectCount; obj++)
            {
                PlayerAnimator.SetTrigger("Attack");
                PlayerSequence(Battle.b.placedSoulObjects[obj]);
                Battle.b.placedSoulObjects[obj].cooldownStart();
                while(PlayerAnimator.attackDone == false)
                {
                    await Task.Delay(1);
                }
                PlayerAnimator.attackDone = false;
            }
            soulObjectCount = 0;
            Battle.b.bs = BattleState.EnemyAction;
            Debug.Log("Enemy Turn");
        }
    }

    public static void EnemyTurn()
    {
        bool allEnemiesDead = true;
        foreach (Enemy e in Battle.b.enemies)
        {
            //attack animation
            if (!e.dead)
            {
                EnemySequence(e);
                allEnemiesDead = false;
            }
        }
        
        //count down the number of turns for buffs
        foreach (Fighter f in Battle.b.fighters) {
            foreach (BuffCounter bc in f.buffLeft) {
                bc.numTurns--;
                if (bc.numTurns == 0) {
                    double prevBuff = f.buff;
                    f.buff -= bc.buff;
                    if (f is Player)
                    {
                        Debug.Log($"Buff ended | Player buff {prevBuff}x -> {f.buff}");
                    }
                    else
                    {
                        Debug.Log($"Buff ended | Enemy buff {prevBuff}x -> {f.buff}");
                    }
                }
            }
            foreach (DefenseBuffCounter bc in f.defenseBuffLeft)
            {
                bc.numTurns--;
                if (bc.numTurns == 0)
                {
                    double prevBuff = f.defenseBuff;
                    f.defenseBuff += bc.defenseBuff;
                    if (f is Player)
                    {
                        Debug.Log($"Defense Buff ended | Player defense buff {1 - prevBuff}x -> {1 - f.defenseBuff}");
                    }
                    else
                    {
                        Debug.Log($"Defense Buff ended | Enemy defense buff {1 - prevBuff}x -> {1 - f.defenseBuff}");
                    }
                }
            }
        }

        if (allEnemiesDead)
        {
            if (FighterController.fighterController.wave < FighterController.fighterController.levelData.enemyWaves.Count)
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    Destroy(e.healthBar);
                    Destroy(e);
                }
                Battle.b.enemies.Clear();
                FighterController.fighterController.wave++;
                FighterController.PlaceFighters();
            }
        }

        //reset soulblocks to original position
        GridFitter.ResetSoulObjects();
        Battle.b.bs = BattleState.Gimmicks;
        BottomDarkener.UndarkenBottom();
        Debug.Log("Grid Fitting");
        Battle.b.turnNumber++;
    }

    static void PlayerSequence(SoulObject s)
    {
        //change later to add frames
        s.ActivateEffect();
        foreach (Enemy e in Battle.b.enemies)
        {
            if (e.health <= 0)
            {
                e.dead = true;
                e.buff = 1;
                e.buffLeft.Clear();
                e.defenseBuffLeft.Clear();
                e.healthBar.gameObject.SetActive(false);
                e.gameObject.SetActive(false);
            }
        }
    }

    static void EnemySequence(Enemy e)
    {
        //randomly runs one of many preset attacks
        Random rand = new Random();
        int i = rand.Next(0, e.numAtk);
        if (e.effects[i].self) {
            e.effects[i].targets.Add(e);
            e.effects[i].ActivateEffect(e);
        }
        else {
            e.effects[i].ActivateEffect(e);
        }
        if (Player.player.health <= 0)
        {
            Player.player.dead = true;
            Player.player.buff = 1;
            Player.player.buffLeft.Clear();
            Player.player.defenseBuffLeft.Clear();
            Player.player.healthBar.gameObject.SetActive(false);
            Player.player.gameObject.SetActive(false);
        }
        
    }
}
