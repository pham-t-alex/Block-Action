using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AfterDamageStatus : Status
{
    public Effect afterDamageEffect;
    public Fighter statusHolder;
    public bool hasUser;

    public AfterDamageStatus(int duration, Effect e, Fighter fighter, bool hasUser)
    {
        numTurns = duration;
        afterDamageEffect = e;
        statusHolder = fighter;
        this.hasUser = hasUser;
    }

    public void ActivateInnerEffect()
    {
        if (afterDamageEffect.targetType == TargetType.Self)
        {
            afterDamageEffect.targets.Add(statusHolder);
        }
        else if (afterDamageEffect.targetType == TargetType.AllEnemies)
        {
            foreach (Enemy enemy in Battle.b.enemies)
            {
                afterDamageEffect.targets.Add(enemy);
            }
        }
        else if (afterDamageEffect.targetType == TargetType.SingleTarget)
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
                afterDamageEffect.targets.Add(Battle.b.enemies[aliveIndices[rand.Next(0, aliveIndices.Count)]]);
            }
            else
            {
                afterDamageEffect.targets.Add(Player.player);
            }
        }
        if (hasUser)
        {
            afterDamageEffect.ActivateEffect(statusHolder);
        }
        else
        {
            afterDamageEffect.ActivateEffect(null);
        }
        afterDamageEffect.targets.Clear();
    }

    public override Quality getQuality()
    {
        if (statusHolder is Player)
        {
            return Effect.GetQuality(afterDamageEffect, true);
        }
        else
        {
            return Effect.GetQuality(afterDamageEffect, false);
        }
    }
}
