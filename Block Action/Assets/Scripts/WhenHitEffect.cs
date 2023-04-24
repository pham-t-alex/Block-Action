using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhenHitEffect : Effect
{
    public int duration;
    public Effect effect;

    public WhenHitEffect(int duration, Effect effect)
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
                WhenHitStatus status = new WhenHitStatus(duration, effect, f);
                f.statusEffects.Add(status);
                if (f.Equals(Player.player))
                {
                    Debug.Log("When-hit effect applied to the player.");
                }
                else
                {
                    Debug.Log("When=hit effect applied to an enemy.");
                }
            }
        }
    }
}
