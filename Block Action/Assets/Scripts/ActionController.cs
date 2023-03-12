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
                Battle.b.placedSoulObjects[obj].showEffect();
                while (PlayerAnimator.attackDone == false)
                {
                    await Task.Yield();
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
        
        

        if (Player.player.dead)
        {
            BattleEndController.TriggerDefeat();
            return;
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
                return;
            }
        }

        //reset soulblocks to original position
        GridFitter.ResetSoulObjects();
        GimmickController.gimmickController.index = 0;
        Battle.b.bs = BattleState.StatusEffects;
        Debug.Log("Status Effects");
        BottomDarkener.UndarkenBottom();
        Battle.b.turnNumber++;
    }

    static void PlayerSequence(SoulObject s)
    {
        //change later to add frames
        s.ActivateEffect();
        Battle.updateDead();
    }

    static void EnemySequence(Enemy e)
    {
        //randomly runs one of many preset attacks
        Random rand = new Random();
        int i = rand.Next(0, e.numAtk);
        Action action = e.actions[i];
        foreach (Effect effect in action.effects)
        {
            if (effect.targetType == TargetType.Self)
            {
                effect.targets.Add(e);
                effect.ActivateEffect(e);
            }
            else if (effect.targetType == TargetType.AllEnemies)
            {
                foreach (Enemy enemy in Battle.b.enemies)
                {
                    effect.targets.Add(enemy);
                }
                effect.ActivateEffect(e);
            }
            else if (effect.targetType == TargetType.SingleTarget)
            {
                effect.targets.Add(Player.player);
                effect.ActivateEffect(e);
            }
            effect.targets.Clear();
        }
        Battle.updateDead();
        
    }
}
