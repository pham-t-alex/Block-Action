using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEffect : Effect
{
    public int delay;
    public Effect effect;
    public bool hasUser;
    public DelayedEffect(int delay, Effect effect, bool hasUser)
    {
        this.delay = delay;
        this.effect = effect;
        this.hasUser = hasUser;
    }
    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                DelayedEffectStatus status = new DelayedEffectStatus(delay, effect, f, hasUser);
                f.statusEffects.Add(status);
                if (f.Equals(Player.player))
                {
                    Debug.Log("Delayed effect applied to the player.");
                }
                else
                {
                    Debug.Log("Delayed effect applied to an enemy.");
                }
            }
        }
    }
}
