using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalingActionEffect : Effect
{
    public Effect effect;
    public Scale scale;
    
    public ScalingActionEffect(Effect e, Scale s)
    {
        effect = e;
        targetType = e.targetType;
        scale = s;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int quantity = Mathf.RoundToInt(scale.ScaledValue(fighter, f));
                effect.targets.Add(f);
                for (int i = 0; i < quantity; i++)
                {
                    effect.ActivateEffect(fighter);
                }
                effect.targets.Clear();
            }
        }
    }
}
