using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status
{
    public int numTurns;

    public virtual void decrementTurns()
    {
        numTurns--;
    }

    public static void TriggerStatusEffects()
    {
        foreach (Fighter f in Battle.b.fighters)
        {
            foreach (Status status in f.statusEffects)
            {
                status.decrementTurns();
            }
            for (int i = 0; i < f.statusEffects.Count; i++)
            {
                if (f.statusEffects[i].numTurns <= 0)
                {
                    f.statusEffects.RemoveAt(i);
                    i--;
                }
            }
        }

        foreach (Fighter f in Battle.b.fighters)
        {
            if (!f.dead)
            {
                foreach (BuffCounter bc in f.buffLeft)
                {
                    bc.numTurns--;
                    if (bc.numTurns == 0)
                    {
                        double prevBuff = f.buff;
                        f.buff -= bc.buff;
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
                foreach (DefenseBuffCounter bc in f.defenseBuffLeft)
                {
                    bc.numTurns--;
                    if (bc.numTurns == 0)
                    {
                        double prevBuff = f.defenseBuff;
                        f.defenseBuff += bc.defenseBuff;
                        if (f is Player)
                        {
                            Debug.Log($"Defense Buff ended | Player defense buff {1 - prevBuff}x -> {1 - f.defenseBuff}");
                        }
                        else
                        {
                            Debug.Log($"Defense Buff ended | Enemy defense buff {1 - prevBuff}x -> {1 - f.defenseBuff}");
                        }
                    }
                }
            }
        }

        Battle.updateDead();
        Battle.b.bs = BattleState.Gimmicks;
        Debug.Log("Mid Level Effects");
    }
}
