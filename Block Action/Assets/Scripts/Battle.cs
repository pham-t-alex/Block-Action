using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    public static Battle b;
    public List<Enemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        b = this;
        bs = BattleState.PlayerGrid;
        Effect e1 = new Damage(50);
        Effect e2 = new Heal(20);
        Effect e3 = new Damage(5);
        e1.self = false;
        e2.self = true;
        e3.self = false;
        soulObjects[0].effects.Add(e1);
        soulObjects[1].effects.Add(e2);
        soulObjects[2].effects.Add(e3);
    }

    // Update is called once per frame
    void Update()
    {
        if (bs.Equals(BattleState.PlayerGrid)) {
            GridFitter.GridFitting();
        } else if (bs.Equals(BattleState.PlayerAction)) {
            PlayerTurn();
        } else if (bs.Equals(BattleState.EnemyAction)) {
            EnemyTurn();
        } else if (bs.Equals(BattleState.EnemySelect)) {
            GridFitter.EnemySelect();
        }
    }

    void PlayerTurn() {
        //take the list of soul blocks placed in the grid
        foreach (SoulObject soulObject in placedSoulObjects) {
            //attack animation
            PlayerSequence(soulObject);
        }
        bs = BattleState.EnemyAction;
        Debug.Log("Enemy Turn");
    }

    void EnemyTurn() {
        foreach (Enemy e in enemies) {
            //attack animation
            EnemySequence(e);
        }
        bs = BattleState.PlayerGrid;
        //reset soulblocks to original position
        GridFitter.ResetSoulObjects();
        Debug.Log("Grid Fitting");
    }

    void PlayerSequence(SoulObject s) {
        //change later to add frames
        //after single target, where do we save the attack enemy?
        s.ActivateEffect();
        foreach (Enemy e in enemies)
        {
            if (e.health <= 0) {
                e.gameObject.SetActive(false);
            }
        }
    }

    //revamp this
    void EnemySequence(Enemy e) {
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
