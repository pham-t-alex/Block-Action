using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status
{
    public int numTurns;

    public virtual void decrementTurns()
    {
        numTurns--;
    }

    public static void TriggerStatusEffects()
    {
        foreach (Status status in Player.player.statusEffects)
        {
            status.decrementTurns();
        }
        for (int i = 0; i < Player.player.statusEffects.Count; i++)
        {
            if (Player.player.statusEffects[i].numTurns <= 0)
            {
                Player.player.statusEffects.RemoveAt(i);
                i--;
            }
        }
        foreach (Enemy enemy in Battle.b.enemies)
        {
            foreach (Status status in enemy.statusEffects)
            {
                status.decrementTurns();
            }
            for (int i = 0; i < enemy.statusEffects.Count; i++)
            {
                if (enemy.statusEffects[i].numTurns <= 0)
                {
                    enemy.statusEffects.RemoveAt(i);
                    i--;
                }
            }
        }
        Battle.updateDead();
        Battle.b.bs = BattleState.PlayerGrid;
        Debug.Log("Grid Fitting");
    }
}
