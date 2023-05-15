using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AfterActionStatus : Status
{
    public Effect afterActionEffect;
    public Fighter statusHolder;
    public bool hasUser;

    public AfterActionStatus(int duration, Effect e, Fighter fighter, bool hasUser)
    {
        numTurns = duration;
        afterActionEffect = e;
        statusHolder = fighter;
        this.hasUser = hasUser;
    }

    public void ActivateInnerEffect()
    {
        if (afterActionEffect.targetType == TargetType.Self)
        {
            afterActionEffect.targets.Add(statusHolder);
        }
        else if (afterActionEffect.targetType == TargetType.AllEnemies)
        {
            foreach (Enemy enemy in Battle.b.enemies)
            {
                afterActionEffect.targets.Add(enemy);
            }
        }
        else if (afterActionEffect.targetType == TargetType.SingleTarget)
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
                afterActionEffect.targets.Add(Battle.b.enemies[aliveIndices[rand.Next(0, aliveIndices.Count)]]);
            }
            else
            {
                afterActionEffect.targets.Add(Player.player);
            }
        }
        if (hasUser)
        {
            afterActionEffect.ActivateEffect(statusHolder);
        }
        else
        {
            afterActionEffect.ActivateEffect(null);
        }
        afterActionEffect.targets.Clear();
    }

    public override Quality getQuality()
    {
        if (statusHolder is Player)
        {
            return Effect.GetQuality(afterActionEffect, true);
        }
        else
        {
            return Effect.GetQuality(afterActionEffect, false);
        }
    }
}
