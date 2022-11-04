using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public List<Effect> effects; // change to effect
    public int numAtk;
    public bool mouseTouching;

    // Start is called before the first frame update
    void Start()
    {
        makeHealthBar();
        health = 100;
        maxHealth = 100;
        buff = 1.0;
        buffLeft = new List<BuffCounter>();

        effects = new List<Effect>();
        //later on, for different types of enemies, make a class that extends enemy, then initialize their effects in their own class
        //e.g. boar would have effects such as Damage, while a preist (just an example) would have effects Heal and Buff. They would all extend enemy
        Effect enemyeff1 = new Damage(10);
        Effect enemyeff2 = new Damage(20);
        Effect enemyeff3 = new Buff(0.5);
        Effect enemyeff4 = new Heal(25);
        enemyeff1.self = false;
        enemyeff2.self = false;
        enemyeff3.self = false;
        enemyeff4.self = false;     //AOE healing for the enemies
        enemyeff3.numTurns = 3;
        enemyeff1.targets.Add(Player.player);
        enemyeff2.targets.Add(Player.player);
        enemyeff3.targets.Add(Player.player);
        //this is def bugged. this only adds preexisting enemies + itself. the preexising enemies do not update their own targets
        foreach (Enemy e in Battle.b.enemies)
        {
            enemyeff4.targets.Add(e);
        }
        enemyeff4.targets.Add(this);
        effects.Add(enemyeff1);
        effects.Add(enemyeff2);
        effects.Add(enemyeff3);
        effects.Add(enemyeff4);
        numAtk = effects.Count;

    }

    // Update is called once per frame
    void Update()
    {
        updateHealthBar();
    }

    void OnMouseEnter()
    {
        mouseTouching = true;
    }

    private void OnMouseExit()
    {
        mouseTouching = false;
    }
}
