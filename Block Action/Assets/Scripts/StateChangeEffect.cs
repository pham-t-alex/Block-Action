using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangeEffect : Effect
{
    public string state;
    public StateChangeEffect(string state)
    {
        this.state = state;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead && f is Enemy)
            {
                ((Enemy)f).state = state;
                Debug.Log("Enemy changed state to: " + state);
            }
        }
    }
}
