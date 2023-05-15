using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DelayedEffectStatus : Status
{
    public Effect delayedEffect;
    public Fighter statusHolder;
    public bool hasUser;
    public DelayedEffectStatus(int duration, Effect e, Fighter fighter, bool hasUser)
    {
        numTurns = duration;
        delayedEffect = e;
        statusHolder = fighter;
        this.hasUser = hasUser;
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
            else if (delayedEffect.targetType == TargetType.SingleTarget)
            {
                if (statusHolder == Player.player)
                {
                    List<int> aliveIndices = new List<int>();
                    for (int i = 0; i < Battle.b.enemies.Count; i++)
                    {
                        if (!Battle.b.enemies[i].dead)
                        {
                            aliveIndices.Add(i);
                        }
                    }
                    if (aliveIndices.Count == 0)
                    {
                        return;
                    }
                    Random rand = new Random();
                    delayedEffect.targets.Add(Battle.b.enemies[rand.Next(0, aliveIndices.Count)]);
                }
                else
                {
                    delayedEffect.targets.Add(Player.player);
                }
            }
            if (hasUser)
            {
                delayedEffect.ActivateEffect(statusHolder);
            }
            else
            {
                delayedEffect.ActivateEffect(null);
            }
            delayedEffect.targets.Clear();
        }
    }

    public override Quality getQuality()
    {
        if (statusHolder is Player)
        {
            return Effect.GetQuality(delayedEffect, true);
        }
        else
        {
            return Effect.GetQuality(delayedEffect, false);
        }
    }
}
