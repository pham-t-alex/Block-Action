using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff : Effect
{
    public int numTurns;
    public double buff;

    public Buff(double buff, int numTurns) {
        this.buff = buff;
        this.numTurns = numTurns;
    }
    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                AtkBuffStatus status = new AtkBuffStatus(numTurns, buff, f);
                f.statusEffects.Add(status);
                double prevBuff = f.buff;
                f.buff += buff;
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player buff set from " + prevBuff + "x to " + f.buff + "x");
                }
                else
                {
                    Debug.Log("Enemy buff set to " + prevBuff + "x to " + f.buff + "x");
                }
            }
        }
    }
}
