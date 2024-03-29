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
        if (effectData[0].Equals("conditional"))
        {
            //Note: example conditional: "conditional health < user target [effect]"
            //Therefore, the conditional should be 4 values: conditional subclass, comparison, first object, second object
            Condition c = Condition.conditionFromStrings(effectData[1], effectData[2], effectData[3], effectData[4]);
            string[] nextEffectData = new string[effectData.Length - 5];
            System.Array.Copy(effectData, 5, nextEffectData, 0, effectData.Length - 5);
            effect = new ConditionalEffect(effectFromStringArray(nextEffectData), c);
            return effect;
        }
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
        else if (effectData[0].Equals("change_state"))
        {
            effect = new StateChangeEffect(string.Join(" ", effectData, 2, effectData.Length - 2));
        }
        else if (effectData[0].Equals("remove_buff"))
        {
            effect = new BuffRemovalEffect(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("remove_debuff"))
        {
            effect = new DebuffRemovalEffect(System.Convert.ToInt32(effectData[2]));
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
        else if (e is StateChangeEffect)
        {
            effectString = $"Change ";
            if (e.targetType == TargetType.Self)
            {
                effectString += "self's"; //should be the most common
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
                { //note: under no circumstances should this happen
                    effectString += "the player's";
                }
            }
            effectString += " state to " + ((StateChangeEffect)e).state;
        }
        else if (e is BuffRemovalEffect)
        {
            effectString = $"Remove {((BuffRemovalEffect)e).quantity} buffs from ";
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
        else if (e is DebuffRemovalEffect)
        {
            effectString = $"Remove {((DebuffRemovalEffect)e).quantity} debuffs from ";
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
        else if (e is ConditionalEffect)
        {
            effectString = ((ConditionalEffect)e).condition.ToString();
            effectString += effectToString(((ConditionalEffect)e).effect, forBlock);
        }
        else
        {
            return null;
        }
        return effectString;
    }

    public static Quality GetQuality(Effect e, bool isPlayer)
    {
        if (e is Damage || e is TrueDamage || e is DefIgnoringDamage || e is BuffRemovalEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.SingleTarget)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
            else
            {
                if (e.targetType == TargetType.SingleTarget)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
        }
        else if (e is Heal || e is DebuffRemovalEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
        }
        else if (e is Buff)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    if (((Buff)e).buff > 0)
                    {
                        return Quality.Good;
                    }
                    else if (((Buff)e).buff < 0)
                    {
                        return Quality.Bad;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
                else
                {
                    if (((Buff)e).buff > 0)
                    {
                        return Quality.Bad;
                    }
                    else if (((Buff)e).buff < 0)
                    {
                        return Quality.Good;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
            }
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    if (((Buff)e).buff > 0)
                    {
                        return Quality.Good;
                    }
                    else if (((Buff)e).buff < 0)
                    {
                        return Quality.Bad;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
                else
                {
                    if (((Buff)e).buff > 0)
                    {
                        return Quality.Bad;
                    }
                    else if (((Buff)e).buff < 0)
                    {
                        return Quality.Good;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
            }
        }
        else if (e is DefenseBuff)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    if (((DefenseBuff)e).defenseBuff > 0)
                    {
                        return Quality.Good;
                    }
                    else if (((DefenseBuff)e).defenseBuff < 0)
                    {
                        return Quality.Bad;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
                else
                {
                    if (((DefenseBuff)e).defenseBuff > 0)
                    {
                        return Quality.Bad;
                    }
                    else if (((DefenseBuff)e).defenseBuff < 0)
                    {
                        return Quality.Good;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
            }
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    if (((DefenseBuff)e).defenseBuff > 0)
                    {
                        return Quality.Good;
                    }
                    else if (((DefenseBuff)e).defenseBuff < 0)
                    {
                        return Quality.Bad;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
                else
                {
                    if (((DefenseBuff)e).defenseBuff > 0)
                    {
                        return Quality.Bad;
                    }
                    else if (((DefenseBuff)e).defenseBuff < 0)
                    {
                        return Quality.Good;
                    }
                    else
                    {
                        return Quality.Neutral;
                    }
                }
            }
        }
        else if (e is DelayedEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    return GetQuality(((DelayedEffect)e).effect, true);
                }
                else
                {
                    Quality q = GetQuality(((DelayedEffect)e).effect, false);
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
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    return GetQuality(((DelayedEffect)e).effect, false);
                }
                else
                {
                    Quality q = GetQuality(((DelayedEffect)e).effect, true);
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
        }
        else if (e is RepeatingEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    return GetQuality(((RepeatingEffect)e).effect, true);
                }
                else
                {
                    Quality q = GetQuality(((RepeatingEffect)e).effect, false);
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
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    return GetQuality(((RepeatingEffect)e).effect, false);
                }
                else
                {
                    Quality q = GetQuality(((RepeatingEffect)e).effect, true);
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
        }
        else if (e is StunEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.SingleTarget)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
            else
            {
                if (e.targetType == TargetType.SingleTarget)
                {
                    return Quality.Good;
                }
                else
                {
                    return Quality.Bad;
                }
            }
        }
        else if (e is AfterActionEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    return GetQuality(((AfterActionEffect)e).effect, true);
                }
                else
                {
                    Quality q = GetQuality(((AfterActionEffect)e).effect, false);
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
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self)
                {
                    return GetQuality(((AfterActionEffect)e).effect, false);
                }
                else
                {
                    Quality q = GetQuality(((AfterActionEffect)e).effect, true);
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
        }
        else if (e is StateChangeEffect)
        {
            return Quality.Neutral;
        }
        else if (e is ConditionalEffect)
        {
            return GetQuality(((ConditionalEffect)e).effect, isPlayer);
        }
        else
        {
            return Quality.Neutral;
        }
    }
}
