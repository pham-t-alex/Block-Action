using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBlockStatus : Status
{
    public HealBlockStatus(int duration)
    {
        numTurns = duration;
    }

    public override Quality getQuality()
    {
        return Quality.Bad;
    }
}
