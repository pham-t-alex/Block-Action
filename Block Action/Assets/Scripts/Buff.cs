using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff : Effect
{
    public int numTurns;
    public double buff;

    public Buff(double buff, int numTurns) {
        this.buff = buff;
        this.numTurns = numTurns;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                BuffCounter bc = new BuffCounter(numTurns, buff);
                f.buffLeft.Add(bc);
                double prevBuff = f.buff;
                f.buff += buff;
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player buff set from " + prevBuff + "x to " + f.buff + "x");
                }
                else
                {
                    Debug.Log("Enemy buff set to " + prevBuff + "x to " + f.buff + "x");
                }
            }
        }
    }
}
