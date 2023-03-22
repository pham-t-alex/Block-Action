using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkBuffStatus : Status
{
    public double buff;
    public Fighter statusHolder;

    public AtkBuffStatus(int duration, double buff, Fighter fighter)
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
        double prevBuff = statusHolder.buff;
        statusHolder.buff -= buff;
        if (statusHolder is Player)
        {
            Debug.Log($"Buff ended | Player buff {prevBuff}x -> {statusHolder.buff}");
        }
        else
        {
            Debug.Log($"Buff ended | Enemy buff {prevBuff}x -> {statusHolder.buff}");
        }
    }
}
