using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public List<Effect> effects; // change to effect
    public int numAtk;
    public bool mouseTouching;
    public string type;
    public int minAction;
    public int maxAction;
    public double atkScale;
    public double buffScale;

    // Start is called before the first frame update
    void Start()
    {
        makeHealthBar();
        /*health = 100;
        maxHealth = 100;
        buff = 1.0;
        buffLeft = new List<BuffCounter>();

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
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            updateHealthBar();
        }
    }

    void OnMouseEnter()
    {
        mouseTouching = true;
    }

    private void OnMouseExit()
    {
        mouseTouching = false;
        DestroyInfoMenu();
        timeHovered = 0;
    }

    public override string GetName()
    {
        string[] nameParts = type.Split("_");
        string fixedName = "";
        for (int i = 0; i < nameParts.Length - 1; i++)
        {
            fixedName += nameParts[i] + " ";
        }
        fixedName += nameParts[nameParts.Length - 1];
        return fixedName;
    }

    public override string GetInfo()
    {
        EnemyData enemyData = Resources.Load<EnemyData>($"EnemyData/{type}");

        string info = "<i>" + enemyData.description + "</i>\n\n";
        info += "Health: " + health + "/" + maxHealth + "\n";
        info += "Status Effects:";
        foreach (BuffCounter bc in buffLeft)
        {
            if (bc.numTurns > 0)
            {
                if (bc.buff > 0)
                {
                    info += "\nAtk +" + (bc.buff * 100) + " % (" + bc.numTurns + " turns)";
                }
                else
                {
                    info += "\nAtk " + (bc.buff * 100) + " % (" + bc.numTurns + " turns)";
                }
            }
        }
        foreach (DefenseBuffCounter bc in defenseBuffLeft)
        {
            if (bc.numTurns > 0)
            {
                if (bc.defenseBuff > 0)
                {
                    info += "\nDef +" + (bc.defenseBuff * 100) + " % (" + bc.numTurns + " turns)";
                }
                else
                {
                    info += "\nDef " + (bc.defenseBuff * 100) + " % (" + bc.numTurns + " turns)";
                }
            }
        }
        info += "\nActions:";
        for (int i = minAction; i <= maxAction; i++)
        {
            string s = ActionAsString(enemyData.actions[i]);
            if (s != null)
            {
                info += "\n- " + s;
            }
        }
        return info;
    }

    public string ActionAsString(string action)
    {
        string[] effectData = action.Split(" ");
        string effectAsString;
        if (effectData[0].Equals("dmg"))
        {
            effectAsString = "Deal " + (System.Convert.ToInt32(effectData[2]) * atkScale) + " damage to ";
        }
        else if (effectData[0].Equals("heal"))
        {
            effectAsString = "Heal ";
        }
        else if (effectData[0].Equals("buff"))
        {
            effectAsString = "Buff ";
        }
        else
        {
            return null;
        }
        if (effectData[1].Equals("player"))
        {
            effectAsString += "the player";
        }
        else if (effectData[1].Equals("self"))
        {
            effectAsString += "this enemy";
        }
        else if (effectData[1].Equals("enemies"))
        {
            effectAsString += "all enemies";
        }
        else
        {
            return null;
        }
        if (effectData[0].Equals("dmg")) {
            effectAsString += ".";
        } 
        else if (effectData[0].Equals("heal"))
        {
            effectAsString += " by " + (System.Convert.ToDouble(effectData[2]) * buffScale) + " HP.";
        }
        else if (effectData[0].Equals("buff"))
        {
            if (effectData[1].Equals("enemies"))
            {
                effectAsString += "'";
            }
            else
            {
                effectAsString += "'s";
            }
            if (effectData[2].Equals("atk"))
            {
                effectAsString += " attack by " + (System.Convert.ToDouble(effectData[3]) * buffScale * 100) + "% for " + System.Convert.ToInt32(effectData[4]) + " turns.";
            }
            else if (effectData[2].Equals("def"))
            {
                effectAsString += " defense by " + (System.Convert.ToDouble(effectData[3]) * buffScale * 100) + "% for " + System.Convert.ToInt32(effectData[4]) + " turns.";
            }
        }
        return effectAsString;
    }
}
