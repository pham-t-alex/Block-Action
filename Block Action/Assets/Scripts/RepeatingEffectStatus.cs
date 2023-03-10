using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingEffectStatus : Status
{
    public Effect repeatingEffect;
    public Fighter statusHolder;

    public RepeatingEffectStatus(int duration, Effect e, Fighter fighter)
    {
        numTurns = duration;
        repeatingEffect = e;
        statusHolder = fighter;
    }

    public override void decrementTurns()
    {
        base.decrementTurns();
        if (numTurns >= 0)
        {
            if (repeatingEffect.targetType == TargetType.Self)
            {
                repeatingEffect.targets.Add(statusHolder);
            }
            else if (repeatingEffect.targetType == TargetType.AllEnemies)
            {
                foreach (Enemy enemy in Battle.b.enemies)
                {
                    repeatingEffect.targets.Add(enemy);
                }
            }
            repeatingEffect.ActivateEffect(null);
            repeatingEffect.targets.Clear();
        }
    }
}
