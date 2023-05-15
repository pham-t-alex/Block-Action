using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterActionEffect : Effect
{
    public int duration;
    public Effect effect;
    public bool hasUser;

    public AfterActionEffect(int duration, Effect effect, bool hasUser)
    {
        this.duration = duration;
        this.effect = effect;
        this.hasUser = hasUser;
    }
    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                AfterActionStatus status = new AfterActionStatus(duration, effect, f, hasUser);
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
