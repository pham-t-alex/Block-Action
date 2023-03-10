using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenseBuff : Effect
{
    public int numTurns;
    public double defenseBuff;

    public DefenseBuff(double defenseBuff, int numTurns)
    {
        this.defenseBuff = defenseBuff;
        this.numTurns = numTurns;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                DefenseBuffCounter bc = new DefenseBuffCounter(numTurns, defenseBuff);
                f.defenseBuffLeft.Add(bc);
                double prevBuff = f.defenseBuff;
                f.defenseBuff -= defenseBuff;
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player defense buff set from " + (1 - prevBuff) + "x to " + (1 - f.defenseBuff) + "x");
                }
                else
                {
                    Debug.Log("Enemy defense buff set to " + (1 - prevBuff) + "x to " + (1 - f.defenseBuff) + "x");
                }
            }
        }
    }
}