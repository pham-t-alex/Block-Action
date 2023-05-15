using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBlockEffect : Effect
{
    public int numTurns;
    // Start is called before the first frame update
    public HealBlockEffect(int numTurns)
    {
        this.numTurns = numTurns;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {

                HealBlockStatus status = new HealBlockStatus(numTurns);
                f.statusEffects.Add(status);
                if (f.Equals(Player.player))
                {
                    Debug.Log("Heal block applied to player");
                }
                else
                {
                    Debug.Log("Heal block applied to enemy");
                }
            }
        }
    }
}
