using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefBuffStatus : Status
{
    public double buff;
    public Fighter statusHolder;

    public DefBuffStatus(int duration, double buff, Fighter fighter)
    {
        numTurns = duration;
        this.buff = buff;
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
        double prevBuff = statusHolder.defenseBuff;
        statusHolder.defenseBuff += buff;
        if (statusHolder is Player)
        {
            Debug.Log($"Defense Buff ended | Player defense buff {1 - prevBuff}x -> {1 - statusHolder.defenseBuff}");
        }
        else
        {
            Debug.Log($"Defense Buff ended | Enemy defense buff {1 - prevBuff}x -> {1 - statusHolder.defenseBuff}");
        }
    }

    public override Quality getQuality()
    {
        if (buff > 0)
        {
            return Quality.Good;
        }
        else if (buff < 0)
        {
            return Quality.Bad;
        }
        else
        {
            return Quality.Neutral;
        }
    }
}