using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntStatus : Status
{
    public Fighter statusHolder;
    public TauntStatus(int duration, Fighter fighter)
    {
        numTurns = duration;
        statusHolder = fighter;
    }

    public override void decrementTurns()
    {
        base.decrementTurns();
        if (numTurns == 0)
        {
            terminate();
        }
    }

    public override void terminate()
    {
        bool stillTaunting = false;
        foreach (Status s in statusHolder.statusEffects)
        {
            if (s is TauntStatus && s != this)
            {
                stillTaunting = true;
            }
        }
        ((Enemy)statusHolder).taunting = stillTaunting;
    }

    public override Quality getQuality()
    {
        return Quality.Neutral;
    }
}
