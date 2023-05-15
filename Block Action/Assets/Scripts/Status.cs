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

    public abstract Quality getQuality();

    public async static void TriggerStatusEffects()
    {
        foreach (Fighter f in Battle.b.fighters)
        {
            int statusEffectsSize = f.statusEffects.Count;
            for (int i = 0; i < statusEffectsSize; i++)
            {
                if (!f.dead)
                {
                    f.statusEffects[i].decrementTurns();
                    if (f.statusEffects[i].numTurns <= 0)
                    {
                        f.statusEffects.RemoveAt(i);
                        statusEffectsSize--;
                        i--;
                    }
                }
                await Battle.UpdateDead();
            }
        }
        if (!Battle.finishedDead())
        {
            Battle.b.turnNumber++;
            Battle.b.bs = BattleState.Gimmicks;
            GimmickController.MidLevelEffects();
            Debug.Log("Mid Level Effects");
        }
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
            string[] nextEffectData = new string[statusData.Length - 4];
            System.Array.Copy(statusData, 4, nextEffectData, 0, statusData.Length - 4);
            if (statusData[2].Equals("user"))
            {
                status = new DelayedEffectStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, true);
            }
            else
            {
                status = new DelayedEffectStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, false);
            }
        }
        else if (statusData[1].Equals("repeating"))
        {
            string[] nextEffectData = new string[statusData.Length - 4];
            System.Array.Copy(statusData, 4, nextEffectData, 0, statusData.Length - 4);
            if (statusData[2].Equals("user"))
            {
                status = new RepeatingEffectStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, true);
            }
            else
            {
                status = new RepeatingEffectStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, false);
            }
        }
        else if (statusData[1].Equals("after_action"))
        {
            string[] nextEffectData = new string[statusData.Length - 4];
            System.Array.Copy(statusData, 4, nextEffectData, 0, statusData.Length - 4);
            if (statusData[2].Equals("user"))
            {
                status = new AfterActionStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, true);
            }
            else
            {
                status = new AfterActionStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, false);
            }
        }
        else if (statusData[1].Equals("after_damage"))
        {
            string[] nextEffectData = new string[statusData.Length - 4];
            System.Array.Copy(statusData, 4, nextEffectData, 0, statusData.Length - 4);
            if (statusData[2].Equals("user"))
            {
                status = new AfterDamageStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, true);
            }
            else
            {
                status = new AfterDamageStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, false);
            }
        }
        else if (statusData[1].Equals("when_hit"))
        {
            string[] nextEffectData = new string[statusData.Length - 4];
            System.Array.Copy(statusData, 4, nextEffectData, 0, statusData.Length - 4);
            if (statusData[2].Equals("user"))
            {
                status = new WhenHitStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, true);
            }
            else
            {
                status = new WhenHitStatus(System.Convert.ToInt32(statusData[3]), Effect.effectFromStringArray(nextEffectData), fighter, false);
            }
        }
        else if (statusData[1].Equals("heal_block"))
        {
            status = new HealBlockStatus(System.Convert.ToInt32(statusData[2]));
        }
        else if (statusData[1].Equals("life_steal"))
        {
            status = new LifeStealStatus(System.Convert.ToInt32(statusData[3]), (float)System.Convert.ToDouble(statusData[2]));
        }
        else if (statusData[1].Equals("taunt"))
        {
            status = new TauntStatus(System.Convert.ToInt32(statusData[2]), fighter);
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
        else if (s is AfterActionStatus)
        {
            AfterActionStatus afterActionStatus = (AfterActionStatus)s;
            statusString += "After-action effect (" + afterActionStatus.numTurns + " turns): ";
            if (afterActionStatus.statusHolder == Player.player)
            {
                statusString += Effect.effectToString(afterActionStatus.afterActionEffect, true);
            }
            else
            {
                statusString += Effect.effectToString(afterActionStatus.afterActionEffect, false);
            }
        }
        else if (s is AfterDamageStatus)
        {
            AfterDamageStatus afterDamageStatus = (AfterDamageStatus)s;
            statusString += "After-damage effect (" + afterDamageStatus.numTurns + " turns): ";
            if (afterDamageStatus.statusHolder == Player.player)
            {
                statusString += Effect.effectToString(afterDamageStatus.afterDamageEffect, true);
            }
            else
            {
                statusString += Effect.effectToString(afterDamageStatus.afterDamageEffect, false);
            }
        }
        else if (s is WhenHitStatus)
        {
            WhenHitStatus whenHitStatus = (WhenHitStatus)s;
            statusString += "When-hit effect (" + whenHitStatus.numTurns + " turns): ";
            if (whenHitStatus.statusHolder == Player.player)
            {
                statusString += Effect.effectToString(whenHitStatus.whenHitEffect, true);
            }
            else
            {
                statusString += Effect.effectToString(whenHitStatus.whenHitEffect, false);
            }
        }
        else if (s is HealBlockStatus)
        {
            HealBlockStatus healBlockStatus = (HealBlockStatus)s;
            statusString += $"Heal block ({healBlockStatus.numTurns} turns)";
        }
        else if (s is LifeStealStatus)
        {
            LifeStealStatus lifeStealStatus = (LifeStealStatus)s;
            statusString += $"Life steal ({lifeStealStatus.scale}x, {lifeStealStatus.numTurns} turns)";
        }
        else if (s is TauntStatus)
        {
            TauntStatus taunt = (TauntStatus)s;
            statusString += $"Taunt ({taunt.numTurns} turns)";
        }
        return statusString;
    }

    public static Quality oppositeQuality(Quality q)
    {
        if (q == Quality.Good)
        {
            return Quality.Bad;
        }
        else if (q == Quality.Bad)
        {
            return Quality.Good;
        }
        return Quality.Neutral;
    }
}

public enum Quality
{
    Good,
    Bad,
    Neutral
}