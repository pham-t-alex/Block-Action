using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealStatus : Status
{
    public float scale;
    public LifeStealStatus(int duration, float scale)
    {
        numTurns = duration;
        this.scale = scale;
    }

    public override Quality getQuality()
    {
        return Quality.Good;
    }
}
