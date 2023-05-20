using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class WhenHitStatus : Status
{
    public Effect whenHitEffect;
    public Fighter statusHolder;
    public bool hasUser;

    public WhenHitStatus(int duration, Effect e, Fighter fighter, bool hasUser)
    {
        numTurns = duration;
        whenHitEffect = e;
        statusHolder = fighter;
        this.hasUser = hasUser;
    }

    public void ActivateInnerEffect()
    {
        if (whenHitEffect.targetType == TargetType.Self)
        {
            whenHitEffect.targets.Add(statusHolder);
        }
        else if (whenHitEffect.targetType == TargetType.AllEnemies)
        {
            foreach (Enemy enemy in Battle.b.enemies)
            {
                whenHitEffect.targets.Add(enemy);
            }
        }
        else if (whenHitEffect.targetType == TargetType.OtherEnemies)
        {
            foreach (Enemy enemy in Battle.b.enemies)
            {
                if (enemy != statusHolder)
                {
                    whenHitEffect.targets.Add(enemy);
                }
            }
        }
        else if (whenHitEffect.targetType == TargetType.SingleTarget)
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
                whenHitEffect.targets.Add(Battle.b.enemies[aliveIndices[rand.Next(0, aliveIndices.Count)]]);
            }
            else
            {
                whenHitEffect.targets.Add(Player.player);
            }
        }
        if (hasUser)
        {
            whenHitEffect.ActivateEffect(statusHolder);
        }
        else
        {
            whenHitEffect.ActivateEffect(null);
        }
        whenHitEffect.targets.Clear();
    }

    public override Quality getQuality()
    {
        if (statusHolder is Player)
        {
            return Effect.GetQuality(whenHitEffect, true);
        }
        else
        {
            return Effect.GetQuality(whenHitEffect, false);
        }
    }
}
