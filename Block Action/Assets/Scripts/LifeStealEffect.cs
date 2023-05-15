using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealEffect : Effect
{
    public int duration;
    public float scale;
    public LifeStealEffect(int duration, float scale)
    {
        this.duration = duration;
        this.scale = scale;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                LifeStealStatus status = new LifeStealStatus(duration, scale);
                f.statusEffects.Add(status);
                if (f.Equals(Player.player))
                {
                    Debug.Log("Life-steal status applied to the player.");
                }
                else
                {
                    Debug.Log("Life-steal status applied to an enemy.");
                }
            }
        }
    }
}
