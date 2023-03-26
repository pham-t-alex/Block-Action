using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public TargetType targetType;
    public List<Fighter> targets = new List<Fighter>();

    public abstract void ActivateEffect(Fighter fighter);

    public static Effect effectFromString(string effectAsString)
    {
        string[] effectData = effectAsString.Split(" ");
        return effectFromStringArray(effectData);
    }

    public static Effect effectFromStringArray(string[] effectData)
    {
        Effect effect = null;
        if (effectData[0].Equals("damage"))
        {
            effect = new Damage(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("true_damage"))
        {
            effect = new TrueDamage(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("def_ignoring_damage"))
        {
            effect = new DefIgnoringDamage(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("heal"))
        {
            effect = new Heal(System.Convert.ToDouble(effectData[2]));
        }
        else if (effectData[0].Equals("buff"))
        {
            if (effectData[2].Equals("atk"))
            {
                effect = new Buff(System.Convert.ToDouble(effectData[3]), System.Convert.ToInt32(effectData[4]));
            }
            else if (effectData[2].Equals("def"))
            {
                effect = new DefenseBuff(System.Convert.ToDouble(effectData[3]), System.Convert.ToInt32(effectData[4]));
            }
        }
        else if (effectData[0].Equals("apply_delayed"))
        {
            string[] nextEffectData = new string[effectData.Length - 3];
            System.Array.Copy(effectData, 3, nextEffectData, 0, effectData.Length - 3);
            effect = new DelayedEffect(System.Convert.ToInt32(effectData[2]), effectFromStringArray(nextEffectData));
        }
        else if (effectData[0].Equals("apply_repeating"))
        {
            string[] nextEffectData = new string[effectData.Length - 3];
            System.Array.Copy(effectData, 3, nextEffectData, 0, effectData.Length - 3);
            effect = new RepeatingEffect(System.Convert.ToInt32(effectData[2]), effectFromStringArray(nextEffectData));
        }
        else if (effectData[0].Equals("stun"))
        {
            effect = new StunEffect(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("apply_after_action"))
        {
            string[] nextEffectData = new string[effectData.Length - 3];
            System.Array.Copy(effectData, 3, nextEffectData, 0, effectData.Length - 3);
            effect = new AfterActionEffect(System.Convert.ToInt32(effectData[2]), effectFromStringArray(nextEffectData));
        }
        if (effectData[1].Equals("self"))
        {
            effect.targetType = TargetType.Self;
        }
        else if (effectData[1].Equals("enemies"))
        {
            effect.targetType = TargetType.AllEnemies;
        }
        else if (effectData[1].Equals("singletarget"))
        {
            effect.targetType = TargetType.SingleTarget;
        }
        return effect;
    }

    //forBlock: True if effect is for a block, false otherwise (for enemy)
    public static string effectToString(Effect e, bool forBlock)
    {
        string effectString = "";
        if (e is Damage || e is TrueDamage || e is DefIgnoringDamage)
        {
            effectString = "Deal ";
            if (e is Damage)
            {
                effectString += ((Damage)e).dmg + " damage";
            }
            else if (e is TrueDamage)
            {
                effectString += ((TrueDamage)e).dmg + " true damage";
            }
            else
            {
                effectString += ((DefIgnoringDamage)e).dmg + " def-ignoring damage";
            }
            effectString += " to ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
        }
        else if (e is Heal)
        {
            effectString = "Heal ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
            effectString += " by " + ((Heal)e).heal + " HP";
        }
        else if (e is Buff)
        {
            if (((Buff)e).buff >= 0)
            {
                effectString = "Buff ";
            }
            else
            {
                effectString = "Debuff ";
            }
            if (e.targetType == TargetType.Self)
            {
                effectString += "self's";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies'";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy's";
                }
                else
                {
                    effectString += "the player's";
                }
            }
            effectString += " attack by " + (((Buff)e).buff * 100) + "% for " + ((Buff)e).numTurns + " turns";
        }
        else if (e is DefenseBuff)
        {
            if (((DefenseBuff)e).defenseBuff >= 0)
            {
                effectString = "Buff ";
            }
            else
            {
                effectString = "Debuff ";
            }
            if (e.targetType == TargetType.Self)
            {
                effectString += "self's";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies'";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy's";
                }
                else
                {
                    effectString += "the player's";
                }
            }
            effectString += " defense by " + (((DefenseBuff)e).defenseBuff * 100) + "% for " + ((DefenseBuff)e).numTurns + " turns";
        }
        else if (e is DelayedEffect)
        {
            effectString = $"Apply delayed effect ({((DelayedEffect)e).delay} turns) to ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
            effectString += ": ";
            effectString += effectToString(((DelayedEffect)e).effect, forBlock);

        }
        else if (e is RepeatingEffect)
        {
            effectString = $"Apply repeating effect ({((RepeatingEffect)e).duration} turns) to ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
            effectString += ": ";
            effectString += effectToString(((RepeatingEffect)e).effect, forBlock);
        }
        else if (e is StunEffect)
        {
            effectString = "Apply " + ((StunEffect)e).stunCharge + " stun charge to ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
        }
        else if (e is AfterActionEffect)
        {
            effectString = $"Apply after-action effect ({((AfterActionEffect)e).duration} turns) to ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self";
            }
            else if (e.targetType == TargetType.AllEnemies)
            {
                effectString += "all enemies";
            }
            else if (e.targetType == TargetType.SingleTarget)
            {
                if (forBlock)
                {
                    effectString += "an enemy";
                }
                else
                {
                    effectString += "the player";
                }
            }
            effectString += ": ";
            effectString += effectToString(((AfterActionEffect)e).effect, forBlock);
        }
        else
        {
            return null;
        }
        return effectString;
    }
}
