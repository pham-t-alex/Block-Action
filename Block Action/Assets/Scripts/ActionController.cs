using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
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
        }
        Battle.b.bs = BattleState.EnemyAction;
        Debug.Log("Enemy Turn");
    }

    public static void EnemyTurn()
    {
        foreach (Enemy e in Battle.b.enemies)
        {
            //attack animation
            EnemySequence(e);
        }
        Battle.b.bs = BattleState.PlayerGrid;
        //reset soulblocks to original position
        GridFitter.ResetSoulObjects();
        Debug.Log("Grid Fitting");
    }

    static void PlayerSequence(SoulObject s)
    {
        //change later to add frames
        //after single target, where do we save the attack enemy?
        s.ActivateEffect();
        foreach (Enemy e in Battle.b.enemies)
        {
            if (e.health <= 0)
            {
                e.gameObject.SetActive(false);
            }
        }
    }

    //revamp this
    static void EnemySequence(Enemy e)
    {
        // do smt
        Random rand = new Random();
        int i = rand.Next(0, 2);
        Player.player.health -= e.attack[i];
        Debug.Log("Enemy deals " + e.attack[i] + " damage to the player | HP: " + (Player.player.health + e.attack[i]) + " -> " + Player.player.health);
        if (Player.player.health <= 0)
        {
            Player.player.gameObject.SetActive(false);
        }
    }
}
