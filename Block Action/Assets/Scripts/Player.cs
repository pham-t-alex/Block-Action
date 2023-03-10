using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private static Player _player;
    public static Player player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
            }
            return _player;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        makeHealthBar();
        health = 100; //might not be valid
        buff = 1.0;
        defenseBuff = 1.0;
        maxHealth = 100;
        buffLeft = new List<BuffCounter>();
        statusEffects = new List<Status>();
        defenseBuffLeft = new List<DefenseBuffCounter>();


        Battle.b.fighters.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        updateHealthBar();
    }

    public override string GetName()
    {
        return "Protagonist";
    }

    public override string GetInfo()
    {
        string info = "Health: " + health + "/" + maxHealth + "\n";
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
        return info;
    }

    public void OnMouseExit()
    {
        DestroyInfoMenu();
        timeHovered = 0;
    }
}
