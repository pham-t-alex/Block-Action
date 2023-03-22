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

    public virtual void terminate()
    {
        numTurns = 0;
    }

    public static void TriggerStatusEffects()
    {
        foreach (Fighter f in Battle.b.fighters)
        {
            int statusEffectsSize = f.statusEffects.Count;
            for (int i = 0; i < statusEffectsSize; i++)
            {
                f.statusEffects[i].decrementTurns();
                if (f.statusEffects[i].numTurns <= 0)
                {
                    f.statusEffects.RemoveAt(i);
                    statusEffectsSize--;
                    i--;
                }
            }
        }

        Battle.updateDead();
        Battle.b.bs = BattleState.Gimmicks;
        Debug.Log("Mid Level Effects");
    }
}
