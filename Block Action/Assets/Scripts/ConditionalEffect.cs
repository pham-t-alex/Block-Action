using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalEffect : Effect
{
    public Effect effect;
    public Condition condition;

    public ConditionalEffect(Effect e, Condition c)
    {
        effect = e;
        targetType = e.targetType;
        condition = c;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                if (condition.Fulfilled(fighter, f))
                {
                    Debug.Log("Condition met.");
                    effect.targets.Add(f);
                    effect.ActivateEffect(fighter);
                    effect.targets.Clear();
                }
                else
                {
                    Debug.Log("Condition failed.");
                }
            }
        }
    }
}
