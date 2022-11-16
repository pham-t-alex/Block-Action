using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuffCounter
{
    public int numTurns;
    public double defenseBuff;

    public DefenseBuffCounter(int numTurns, double defenseBuff)
    {
        this.numTurns = numTurns;
        this.defenseBuff = defenseBuff;
    }
}
