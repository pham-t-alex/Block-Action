using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEffectStatus : Status
{
    public Effect delayedEffect;
    public Fighter statusHolder;
    public DelayedEffectStatus(int duration, Effect e, Fighter fighter)
    {
        numTurns = duration;
        delayedEffect = e;
        statusHolder = fighter;
    }

    public override void decrementTurns()
    {
        base.decrementTurns();
        if (numTurns == 0)
        {
            if (delayedEffect.targetType == TargetType.Self)
            {
                delayedEffect.targets.Add(statusHolder);
            }
            else if (delayedEffect.targetType == TargetType.AllEnemies)
            {
                foreach (Enemy enemy in Battle.b.enemies)
                {
                    delayedEffect.targets.Add(enemy);
                }
            }
            delayedEffect.ActivateEffect(null);
            delayedEffect.targets.Clear();
        }
    }
}
