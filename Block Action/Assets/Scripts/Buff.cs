using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Effect
{
    public double buff;

    public Buff(double buff) {
        this.buff = buff;
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
            BuffCounter bc = new BuffCounter(numTurns, buff);
            f.buffLeft.Add(bc);
            f.buff *= buff;
            if (f.Equals(Player.player))
            {
                Debug.Log("Player buff set to " + f.buff + "x");
            }
            else
            {
                Debug.Log("Enemy buff set to " + f.buff + "x");
            }
        }
    }
}
