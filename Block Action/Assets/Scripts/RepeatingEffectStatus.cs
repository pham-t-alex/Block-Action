using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RepeatingEffectStatus : Status
{
    public Effect repeatingEffect;
    public Fighter statusHolder;
    public bool hasUser;

    public RepeatingEffectStatus(int duration, Effect e, Fighter fighter, bool hasUser)
    {
        numTurns = duration;
        repeatingEffect = e;
        statusHolder = fighter;
        this.hasUser = hasUser;
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
            else if (repeatingEffect.targetType == TargetType.SingleTarget)
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
                    repeatingEffect.targets.Add(Battle.b.enemies[aliveIndices[rand.Next(0, aliveIndices.Count)]]);
                }
                else
                {
                    repeatingEffect.targets.Add(Player.player);
                }
            }
            if (hasUser)
            {
                repeatingEffect.ActivateEffect(statusHolder);
            }
            else
            {
                repeatingEffect.ActivateEffect(null);
            }
            repeatingEffect.targets.Clear();
        }
    }

    public override Quality getQuality()
    {
        if (statusHolder is Player)
        {
            return Effect.GetQuality(repeatingEffect, true);
        }
        else
        {
            return Effect.GetQuality(repeatingEffect, false);
        }
    }
}
