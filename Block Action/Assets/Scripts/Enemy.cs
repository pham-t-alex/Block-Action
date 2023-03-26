using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public List<Action> actions; // change to effect
    public int numAtk;
    public bool mouseTouching;
    public string type;
    public int minAction;
    public int maxAction;
    public double atkScale;
    public double buffScale;
    public int actionCount;

    // Start is called before the first frame update
    void Start()
    {
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

        string info = "Health: " + health + "/" + maxHealth + "\n";
        info += "Stun Charge: " + stunCharge + "/" + stunChargeMax + "\n";
        info += "Status Effects:";
        foreach (Status status in statusEffects)
        {
            string s = Status.statusToString(status);
            if (s != null)
            {
                info += "\n- " + s + ".";
            }
        }
        info += "\nActions:";
        for (int i = 0; i < actions.Count; i++)
        {
            string s = ActionAsString(actions[i]);
            if (s != null)
            {
                info += "\n- " + s + ".";
            }
        }
        return info;
    }

    public string ActionAsString(Action action)
    {
        List<Effect> actionData = action.effects;
        string actionAsString = "";
        bool firstIterationDone = false;
        foreach (Effect effect in actionData)
        {
            if (firstIterationDone)
            {
                actionAsString += "; ";
            }
            else
            {
                firstIterationDone = true;
            }
            string effectAsString = Effect.effectToString(effect, false);
            actionAsString += effectAsString;
        }
        return actionAsString;
    }
}
