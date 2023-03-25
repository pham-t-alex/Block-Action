using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status
{
    public int numTurns;
    public bool removable = true;

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
            if (Battle.updateDead())
            {
                return;
            }
        }

        Battle.b.bs = BattleState.Gimmicks;
        Debug.Log("Mid Level Effects");
    }

    public static Status statusFromString(string statusAsString, Enemy fighter)
    {
        string[] statusData = statusAsString.Split(" ");
        Status status = null;
        bool removable;
        if (statusData[0].Equals("unremovable"))
        {
            removable = false;
        }
        else
        {
            removable = true;
        }
        if (statusData[1].Equals("buff"))
        {
            if (statusData[2].Equals("atk"))
            {
                double buff = System.Convert.ToDouble(statusData[3]);
                fighter.buff += buff;
                status = new AtkBuffStatus(System.Convert.ToInt32(statusData[4]), buff, fighter);
            }
            else if (statusData[2].Equals("def"))
            {
                double buff = System.Convert.ToDouble(statusData[3]);
                fighter.defenseBuff -= buff;
                status = new DefBuffStatus(System.Convert.ToInt32(statusData[4]), buff, fighter);
            }
        }
        else if (statusData[1].Equals("delayed"))
        {
            string[] nextEffectData = new string[statusData.Length - 3];
            System.Array.Copy(statusData, 3, nextEffectData, 0, statusData.Length - 3);
            status = new DelayedEffectStatus(System.Convert.ToInt32(statusData[2]), Effect.effectFromStringArray(nextEffectData), fighter);
        }
        else if (statusData[1].Equals("repeating"))
        {
            string[] nextEffectData = new string[statusData.Length - 3];
            System.Array.Copy(statusData, 3, nextEffectData, 0, statusData.Length - 3);
            status = new RepeatingEffectStatus(System.Convert.ToInt32(statusData[2]), Effect.effectFromStringArray(nextEffectData), fighter);
        }
        status.removable = removable;
        return status;
    }

    public static string statusToString(Status s)
    {
        string statusString = "";
        if (!s.removable)
        {
            statusString += "[Unremovable] ";
        }
        if (s is AtkBuffStatus)
        {
            AtkBuffStatus atkBuff = (AtkBuffStatus)s;
            statusString += "Atk ";
            double buff = atkBuff.buff;
            if (buff >= 0)
            {
                statusString += "+" + (buff * 100);
            }
            else
            {
                statusString += (buff * 100);
            }
            statusString += "% (" + atkBuff.numTurns + " turns)";
        }
        else if (s is DefBuffStatus)
        {
            DefBuffStatus defBuff = (DefBuffStatus)s;
            statusString += "Def ";
            double buff = defBuff.buff;
            if (buff >= 0)
            {
                statusString += "+" + (buff * 100);
            }
            else
            {
                statusString += (buff * 100);
            }
            statusString += "% (" + defBuff.numTurns + " turns)";
        }
        else if (s is DelayedEffectStatus)
        {
            DelayedEffectStatus delayedStatus = (DelayedEffectStatus)s;
            statusString += "Delayed effect (" + delayedStatus.numTurns + " turns): ";
            if (delayedStatus.statusHolder == Player.player)
            {
                statusString += Effect.effectToString(delayedStatus.delayedEffect, true);
            }
            else
            {
                statusString += Effect.effectToString(delayedStatus.delayedEffect, false);
            }
        }
        else if (s is RepeatingEffectStatus)
        {
            RepeatingEffectStatus repeatingStatus = (RepeatingEffectStatus)s;
            statusString += "Repeating effect (" + repeatingStatus.numTurns + " turns): ";
            if (repeatingStatus.statusHolder == Player.player)
            {
                statusString += Effect.effectToString(repeatingStatus.repeatingEffect, true);
            }
            else
            {
                statusString += Effect.effectToString(repeatingStatus.repeatingEffect, false);
            }
        }
        return statusString;
    }
}
