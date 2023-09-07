using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingBuffEffect : Effect
{
    public Scale scale;
    public enum BuffType
    {
        Atk,
        Def
    }
    public BuffType buffType;
    public enum ScaleType
    {
        Strength,
        Length
    }
    public ScaleType scaleType;
    public double buffOrDuration;

    public ScalingBuffEffect(Scale s, string buffType, string scaleType, double otherValue)
    {
        scale = s;
        if (buffType.Equals("def"))
        {
            this.buffType = BuffType.Def;
        }
        else
        {
            this.buffType = BuffType.Atk;
        }
        if (scaleType.Equals("length"))
        {
            this.scaleType = ScaleType.Length;
        }
        else
        {
            this.scaleType = ScaleType.Strength;
        }
        buffOrDuration = otherValue;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                Effect e = null;
                if (scaleType == ScaleType.Strength)
                {
                    if (buffType == BuffType.Atk)
                    {
                        e = new Buff(scale.ScaledValue(fighter, f), (int)buffOrDuration);
                    }
                    else
                    {
                        e = new DefenseBuff(scale.ScaledValue(fighter, f), (int)buffOrDuration);
                    }
                }
                else
                {
                    if (buffType == BuffType.Atk)
                    {
                        e = new Buff(buffOrDuration, Mathf.RoundToInt(scale.ScaledValue(fighter, f)));
                    }
                    else
                    {
                        e = new DefenseBuff(buffOrDuration, Mathf.RoundToInt(scale.ScaledValue(fighter, f)));
                    }
                }
                e.targets.Add(f);
                e.ActivateEffect(fighter);
                e.targets.Clear();
            }
        }
    }
}
