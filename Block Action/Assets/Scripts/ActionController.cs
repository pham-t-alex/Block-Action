using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ActionController : MonoBehaviour
{
    //private EnemyAnimator enemyAnimator;

    //references player
    static PlayerAnimator playerAnimator;
    public GameObject player;

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
        //if no blocks were placed in the grid, Attack animation does not play
        if (Battle.b.placedSoulObjects.Count > 0 || PersistentDataManager.levelNumber == 0)
        {
            SoulObjectAnimation();
        }
        else
        {
            if (Player.player.stunned)
            {
                Player.player.stunCharge = 0;
            }
            Battle.b.bs = BattleState.EnemyAction;
            EnemyTurn();
            Debug.Log("Enemy Turn");
        }
    }

    public async static void SoulObjectAnimation()
    {
        if (Player.player.stunned)
        {
            Player.player.stunCharge = 0;
            Battle.b.bs = BattleState.EnemyAction;
            Debug.Log("Enemy Turn");
        }
        else if (Battle.b.placedSoulObjects.Count > 0)
        {
            ActionUserParticle.actionUserParticle.setAction(Player.player);
            for (int obj = 0; obj < Battle.b.placedSoulObjects.Count; obj++)
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
                await Battle.UpdateDead();
            }
            if (!Battle.finishedDead())
            {
                Battle.b.bs = BattleState.EnemyAction;
                ActionUserParticle.actionUserParticle.disable();
                EnemyTurn();
                Debug.Log("Enemy Turn");
            }
        }
        else if (PersistentDataManager.levelNumber == 0)
        {
            Effect effect = new Damage(0);
            foreach (Enemy e in Battle.b.enemies)
            {
                effect.targets.Add(e);
            }
            effect.ActivateEffect(null);
            await Task.Delay(500);
            Battle.b.bs = BattleState.EnemyAction;
            ActionUserParticle.actionUserParticle.disable();
            EnemyTurn();
        }
    }

    public async static void EnemyTurn()
    {
        for (int i = 0; i < Battle.b.enemies.Count; i++)
        {
            Enemy e = Battle.b.enemies[i];
            if (e.stunned)
            {
                e.stunCharge = 0;
                continue;
            }
            for (int j = 0; j < e.actionCount; j++)
            {
                if (!e.dead && !e.stunned)
                {
                    await EnemySequence(e);
                }
            }
            //attack animation
            //EnemyAnimator.SetTrigger("Attack");
        }
        if (!Battle.finishedDead())
        {
            //reset soulblocks to original position
            ActionUserParticle.actionUserParticle.disable();
            GridFitter.ResetSoulObjects();
            GimmickController.gimmickController.index = 0;
            Battle.b.bs = BattleState.StatusEffects;
            Debug.Log("Status Effects");
            Status.TriggerStatusEffects();
        }
    }

    static void PlayerSequence(SoulObject s)
    {
        //change later to add frames
        s.ActivateEffect();
        TriggerAfterActionEffects(Player.player);
    }

    async static Task EnemySequence(Enemy e)
    {
        ActionUserParticle.actionUserParticle.setAction(e);
        await Task.Delay(500);
        //randomly runs one of many preset attacks
        Random rand = new Random();
        int numAtk = getAttackCount(e);
        if (numAtk <= 0)
        {
            return;
        }
        int i = rand.Next(0, numAtk);
        Action action = null;
        if (e.actionSets.ContainsKey("All"))
        {
            if (i < e.actionSets["All"].Count)
            {
                action = e.actionSets["All"][i];
            }
            else
            {
                action = e.actionSets[e.state][i - e.actionSets["All"].Count];
            }
        }
        else
        {
            action = e.actionSets[e.state][i];
        }
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
        TriggerAfterActionEffects(e);
        await Battle.UpdateDead();
        await Task.Delay(500);
    }

    static int getAttackCount(Enemy e)
    {
        int atkCount = 0;
        if (e.actionSets.ContainsKey("All"))
        {
            atkCount += e.actionSets["All"].Count;
        }
        if (e.actionSets.ContainsKey(e.state))
        {
            atkCount += e.actionSets[e.state].Count;
        }
        return atkCount;
    }

    static void TriggerAfterActionEffects(Fighter f)
    {
        for (int i = 0; i < f.statusEffects.Count; i++)
        {
            if (f.statusEffects[i] is AfterActionStatus)
            {
                ((AfterActionStatus)f.statusEffects[i]).ActivateInnerEffect();
            }
        }
    }

    public static void TriggerWhenHitEffects(Fighter f)
    {
        for (int i = 0; i < f.statusEffects.Count; i++)
        {
            if (f.statusEffects[i] is WhenHitStatus)
            {
                ((WhenHitStatus)f.statusEffects[i]).ActivateInnerEffect();
            }
        }
    }

    public static void TriggerAfterDamageEffects(Fighter f)
    {
        for (int i = 0; i < f.statusEffects.Count; i++)
        {
            if (f.statusEffects[i] is AfterDamageStatus)
            {
                ((AfterDamageStatus)f.statusEffects[i]).ActivateInnerEffect();
            }
        }
    }

    public static void TriggerLifeStealEffects(Fighter f, int damage)
    {
        for (int i = 0; i < f.statusEffects.Count; i++)
        {
            if (f.statusEffects[i] is LifeStealStatus)
            {
                Heal h = new(damage * ((LifeStealStatus)f.statusEffects[i]).scale);
                h.targets.Add(f);
                h.ActivateEffect(null);
            }
        }
    }
}
