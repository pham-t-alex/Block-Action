using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
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
        //take the list of soul blocks placed in the grid
        foreach (SoulObject soulObject in Battle.b.placedSoulObjects)
        {
            //attack animation
            PlayerSequence(soulObject);
            playerAnimator.SetTrigger("Attack");
        }
        Battle.b.bs = BattleState.EnemyAction;
        Debug.Log("Enemy Turn");
    }

    public static void EnemyTurn()
    {
        foreach (Enemy e in Battle.b.enemies)
        {
            //attack animation
            if (!e.dead)
            {
                EnemySequence(e);
            }
        }
        Battle.b.bs = BattleState.PlayerGrid;
        //count down the number of turns for buffs
        foreach (Fighter f in Battle.b.fighters) {
            foreach (BuffCounter bc in f.buffLeft) {
                bc.numTurns--;
                if (bc.numTurns == 0) {
                    double prevBuff = f.buff;
                    f.buff /= bc.buff;
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
        }

        //reset soulblocks to original position
        GridFitter.ResetSoulObjects();
        Debug.Log("Grid Fitting");
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
                e.healthBar.gameObject.SetActive(false);
                e.gameObject.SetActive(false);
            }
        }
    }

    //revamp this
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
        //Player.player.health -= e.attack[i];
        // Debug.Log("Enemy deals " + e.attack[i] + " damage to the player | HP: " + (Player.player.health + e.attack[i]) + " -> " + Player.player.health);
        if (Player.player.health <= 0)
        {
            Player.player.dead = true;
            Player.player.buff = 1;
            Player.player.buffLeft.Clear();
            Player.player.healthBar.gameObject.SetActive(false);
            Player.player.gameObject.SetActive(false);
        }
        playerAnimator.SetTrigger("Hurt");
    }
}
