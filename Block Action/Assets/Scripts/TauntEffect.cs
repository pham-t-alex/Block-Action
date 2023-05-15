using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntEffect : Effect
{
    //NOTE: TAUNT SHOULD BE ENEMY ONLY EFFECT
    public int duration;
    public TauntEffect(int duration)
    {
        this.duration = duration;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead && f is Enemy)
            {
                TauntStatus status = new TauntStatus(duration, f);
                f.statusEffects.Add(status);
                ((Enemy)f).taunting = true;
                Debug.Log("Taunt applied to enemy");
            }
        }
    }
}
