using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterActionEffect : Effect
{
    public int duration;
    public Effect effect;

    public AfterActionEffect(int duration, Effect effect)
    {
        this.duration = duration;
        this.effect = effect;
    }
    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                AfterActionStatus status = new AfterActionStatus(duration, effect, f);
                f.statusEffects.Add(status);
                if (f.Equals(Player.player))
                {
                    Debug.Log("After-action effect applied to the player.");
                }
                else
                {
                    Debug.Log("After-action effect applied to an enemy.");
                }
            }
        }
    }
}
