using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public TargetType targetType;
    public List<Fighter> targets = new List<Fighter>();
    public int numTurns;    //for buffing


    public abstract void ActivateEffect(Fighter fighter);
}
