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

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                DefBuffStatus status = new DefBuffStatus(numTurns, defenseBuff, f);
                f.statusEffects.Add(status);
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