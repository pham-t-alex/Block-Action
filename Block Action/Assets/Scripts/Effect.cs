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
        if (effectData[0].Equals("scaled_effect"))
        {
            Scale s = Scale.scaleFromData(effectData[2], effectData[3], effectData[4], (float) System.Convert.ToDouble(effectData[5]), (float)System.Convert.ToDouble(effectData[6]), (float)System.Convert.ToDouble(effectData[7]), (float)System.Convert.ToDouble(effectData[8]));
            if (effectData[1].Equals("action"))
            {
                //Note: example scaled effect: "scaled_effect action health user percentage 0 100 1 4 [effect]"
                string[] nextEffectData = new string[effectData.Length - 9];
                System.Array.Copy(effectData, 9, nextEffectData, 0, effectData.Length - 9);
                effect = new ScalingActionEffect(effectFromStringArray(nextEffectData), s);
            }
            else if (effectData[1].Equals("buff"))
            {
                //Note: example scaled effect: "scaled_effect buff health user percentage 0 100 1 4 [targetType] atk length 0.2" - 0.2 being othervalue
                effect = new ScalingBuffEffect(s, effectData[10], effectData[11], System.Convert.ToDouble(effectData[12]));
                Debug.Log(effectData[11]);
                if (effectData[9].Equals("self"))
                {
                    effect.targetType = TargetType.Self;
                }
                else if (effectData[9].Equals("enemies"))
                {
                    effect.targetType = TargetType.AllEnemies;
                }
                else if (effectData[9].Equals("singletarget"))
                {
                    effect.targetType = TargetType.SingleTarget;
                }
                else if (effectData[9].Equals("others"))
                {
                    effect.targetType = TargetType.OtherEnemies;
                }
            }
            return effect;
        }
        if (effectData[0].Equals("lock_tiles"))
        {
            effect = new GridLockingEffect(System.Convert.ToInt32(effectData[1]), System.Convert.ToInt32(effectData[2]));
            effect.targetType = TargetType.Self;
            return effect;
        }
        if (effectData[0].Equals("summon"))
        {
            string[] nextEffectData = new string[effectData.Length - 1];
            System.Array.Copy(effectData, 1, nextEffectData, 0, effectData.Length - 1);
            effect = new SummonEffect(nextEffectData);
            effect.targetType = TargetType.Self;
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
            string[] nextEffectData = new string[effectData.Length - 4];
            System.Array.Copy(effectData, 4, nextEffectData, 0, effectData.Length - 4);
            if (effectData[2].Equals("user"))
            {
                effect = new DelayedEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), true);
            }
            else
            {
                effect = new DelayedEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), false);
            }
        }
        else if (effectData[0].Equals("apply_repeating"))
        {
            string[] nextEffectData = new string[effectData.Length - 4];
            System.Array.Copy(effectData, 4, nextEffectData, 0, effectData.Length - 4);
            if (effectData[2].Equals("user"))
            {
                effect = new RepeatingEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), true);
            }
            else
            {
                effect = new RepeatingEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), false);
            }
        }
        else if (effectData[0].Equals("stun"))
        {
            effect = new StunEffect(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("apply_after_action"))
        {
            string[] nextEffectData = new string[effectData.Length - 4];
            System.Array.Copy(effectData, 4, nextEffectData, 0, effectData.Length - 4);
            if (effectData[2].Equals("user"))
            {
                effect = new AfterActionEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), true);
            }
            else
            {
                effect = new AfterActionEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), false);
            }
        }
        else if (effectData[0].Equals("apply_after_damage"))
        {
            string[] nextEffectData = new string[effectData.Length - 4];
            System.Array.Copy(effectData, 4, nextEffectData, 0, effectData.Length - 4);
            if (effectData[2].Equals("user"))
            {
                effect = new AfterDamageEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), true);
            }
            else
            {
                effect = new AfterDamageEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), false);
            }
        }
        else if (effectData[0].Equals("apply_when_hit"))
        {
            string[] nextEffectData = new string[effectData.Length - 4];
            System.Array.Copy(effectData, 4, nextEffectData, 0, effectData.Length - 4);
            if (effectData[2].Equals("user"))
            {
                effect = new WhenHitEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), true);
            }
            else
            {
                effect = new WhenHitEffect(System.Convert.ToInt32(effectData[3]), effectFromStringArray(nextEffectData), false);
            }
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
        else if (effectData[0].Equals("apply_element"))
        {
            if (effectData[2] == "fire")
            {
                effect = new ElementalApplicationEffect(Element.Elements.FIRE, System.Convert.ToInt32(effectData[3]));
            }
            else if (effectData[2] == "water")
            {
                effect = new ElementalApplicationEffect(Element.Elements.WATER, System.Convert.ToInt32(effectData[3]));
            }
            else if (effectData[2] == "nature")
            {
                effect = new ElementalApplicationEffect(Element.Elements.NATURE, System.Convert.ToInt32(effectData[3]));
            }
            else
            {
                effect = new ElementalApplicationEffect(Element.Elements.ELEMENTLESS, System.Convert.ToInt32(effectData[3]));
            }
        }
        else if (effectData[0].Equals("apply_heal_block"))
        {
            effect = new HealBlockEffect(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("apply_life_steal"))
        {
            //e.g. apply_life_steal self 0.5 3
            effect = new LifeStealEffect(System.Convert.ToInt32(effectData[3]), (float)System.Convert.ToDouble(effectData[2]));
        }
        else if (effectData[0].Equals("apply_taunt"))
        {
            effect = new TauntEffect(System.Convert.ToInt32(effectData[2]));
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
        else if (effectData[1].Equals("others")) //should be enemy-exclusive for now
        {
            effect.targetType = TargetType.OtherEnemies;
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies'";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies'";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is AfterActionEffect || e is AfterDamageEffect || e is WhenHitEffect)
        {
            if (e is AfterActionEffect)
            {
                effectString = $"Apply after-action effect ({((AfterActionEffect)e).duration} turns) to ";
            }
            else if (e is AfterDamageEffect)
            {
                effectString = $"Apply after-damage effect ({((AfterDamageEffect)e).duration} turns) to ";
            }
            else
            {
                effectString = $"Apply when-hit effect ({((WhenHitEffect)e).duration} turns) to ";
            }
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
            effectString += ": ";
            if (e is AfterActionEffect)
            {
                effectString += effectToString(((AfterActionEffect)e).effect, forBlock);
            }
            else if (e is AfterDamageEffect)
            {
                effectString += effectToString(((AfterDamageEffect)e).effect, forBlock);
            }
            else
            {
                effectString += effectToString(((WhenHitEffect)e).effect, forBlock);
            }
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is ConditionalEffect)
        {
            effectString = ((ConditionalEffect)e).condition.ToString();
            effectString += effectToString(((ConditionalEffect)e).effect, forBlock);
        }
        else if (e is ScalingActionEffect)
        {
            effectString = effectToString(((ScalingActionEffect)e).effect, forBlock);
            effectString += " (# occurrences scaled by ";
            effectString += ((ScalingActionEffect)e).scale.ToString();
            effectString += ")";
        }
        else if (e is ElementalApplicationEffect)
        {
            effectString = $"Apply ";
            switch (((ElementalApplicationEffect)e).element)
            {
                case Element.Elements.FIRE:
                    effectString += "Fire";
                    break;
                case Element.Elements.WATER:
                    effectString += "Water";
                    break;
                case Element.Elements.NATURE:
                    effectString += "Nature";
                    break;
                case Element.Elements.ELEMENTLESS:
                    effectString += "Elementless";
                    break;
            }
            effectString += $" x{((ElementalApplicationEffect)e).stackCount} to ";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is HealBlockEffect)
        {
            effectString = $"Apply heal block ({((HealBlockEffect)e).numTurns} turns) to ";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is LifeStealEffect)
        {
            effectString = $"Apply life steal ({((LifeStealEffect)e).scale}x, {((LifeStealEffect)e).duration} turns) to ";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is TauntEffect)
        {
            effectString = $"Apply taunt ({((TauntEffect)e).duration} turns) to ";
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
            else if (e.targetType == TargetType.OtherEnemies)
            {
                effectString += "other enemies";
            }
        }
        else if (e is ScalingBuffEffect)
        {
            ScalingBuffEffect buffEffect = (ScalingBuffEffect)e;
            effectString = $"Apply ";
            if (buffEffect.buffType == ScalingBuffEffect.BuffType.Def)
            {
                effectString += "def modifier";
            }
            else
            {
                effectString += "atk modifier";
            }
            if (buffEffect.scaleType == ScalingBuffEffect.ScaleType.Strength)
            {
                effectString += ", scaled by " + buffEffect.scale.ToString();
                effectString += $" ({buffEffect.buffOrDuration} turns)";
            }
            else
            {
                effectString += " of ";
                if (buffEffect.buffOrDuration > 0)
                {
                    effectString += "+";
                }
                effectString += $"{buffEffect.buffOrDuration * 100}% for a duration scaled by {buffEffect.scale.ToString()}";
            }
        }
        else if (e is GridLockingEffect)
        {
            effectString = $"Lock {((GridLockingEffect)e).count} tiles for {((GridLockingEffect)e).duration} turns";
        }
        else if (e is SummonEffect)
        {
            effectString = "Summon an enemy into the battlefield";
        }
        else
        {
            return null;
        }
        return effectString;
    }

    public static Quality GetQuality(Effect e, bool isPlayer)
    {
        if (e is Damage || e is TrueDamage || e is DefIgnoringDamage || e is BuffRemovalEffect || e is StunEffect || e is HealBlockEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.SingleTarget || e.targetType == TargetType.OtherEnemies)
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
        else if (e is Heal || e is DebuffRemovalEffect || e is LifeStealEffect)
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
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
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
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
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
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
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
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
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
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
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
        else if (e is AfterActionEffect || e is AfterDamageEffect || e is WhenHitEffect)
        {
            if (isPlayer)
            {
                if (e.targetType == TargetType.Self)
                {
                    if (e is AfterActionEffect)
                    {
                        return GetQuality(((AfterActionEffect)e).effect, true);
                    }
                    else if (e is AfterDamageEffect)
                    {
                        return GetQuality(((AfterDamageEffect)e).effect, true);
                    }
                    else
                    {
                        return GetQuality(((WhenHitEffect)e).effect, true);
                    }
                    
                }
                else
                {
                    if (e is AfterActionEffect)
                    {
                        return Status.oppositeQuality(GetQuality(((AfterActionEffect)e).effect, false));
                    }
                    else if (e is AfterDamageEffect)
                    {
                        return Status.oppositeQuality(GetQuality(((AfterDamageEffect)e).effect, false));
                    }
                    else
                    {
                        return Status.oppositeQuality(GetQuality(((WhenHitEffect)e).effect, false));
                    }
                }
            }
            else
            {
                if (e.targetType == TargetType.AllEnemies || e.targetType == TargetType.Self || e.targetType == TargetType.OtherEnemies)
                {
                    if (e is AfterActionEffect)
                    {
                        return GetQuality(((AfterActionEffect)e).effect, false);
                    }
                    else if (e is AfterDamageEffect)
                    {
                        return GetQuality(((AfterDamageEffect)e).effect, false);
                    }
                    else
                    {
                        return GetQuality(((WhenHitEffect)e).effect, false);
                    }
                }
                else
                {
                    if (e is AfterActionEffect)
                    {
                        return Status.oppositeQuality(GetQuality(((AfterActionEffect)e).effect, true));
                    }
                    else if (e is AfterDamageEffect)
                    {
                        return Status.oppositeQuality(GetQuality(((AfterDamageEffect)e).effect, true));
                    }
                    else
                    {
                        return Status.oppositeQuality(GetQuality(((WhenHitEffect)e).effect, true));
                    }
                }
            }
        }
        else if (e is StateChangeEffect || e is TauntEffect || e is ScalingBuffEffect) //got lazy on Scaling Buff Effect
        {
            return Quality.Neutral;
        }
        else if (e is ConditionalEffect)
        {
            return GetQuality(((ConditionalEffect)e).effect, isPlayer);
        }
        else if (e is ScalingActionEffect)
        {
            return GetQuality(((ScalingActionEffect)e).effect, isPlayer);
        }
        else if (e is GridLockingEffect || e is SummonEffect)
        {
            if (isPlayer)
            {
                return Quality.Bad;
            }
            else
            {
                return Quality.Good;
            }
        }
        else
        {
            return Quality.Neutral;
            //ElementalApplication
        }
    }
}
