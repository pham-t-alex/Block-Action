using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEffect : Effect
{
    public int stunCharge;
    public StunEffect(int stunCharge)
    {
        this.stunCharge = stunCharge;
    }
    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead && !f.stunned)
            {
                int prevStunCharge = f.stunCharge;
                f.stunCharge += stunCharge;
                if (f.stunCharge > f.stunChargeMax)
                {
                    f.stunCharge = f.stunChargeMax;
                }
                if (f.stunCharge < 0)
                {
                    f.stunCharge = 0;
                }
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player's stun charge changed from " + prevStunCharge + " -> " + f.stunCharge);
                }
                else
                {
                    Debug.Log("Enemy's stun charge changed from " + prevStunCharge + " -> " + f.stunCharge);
                }
                if (f.stunned)
                {
                    if (f.Equals(Player.player))
                    {
                        Debug.Log("Player has been stunned");
                        foreach (SoulObject s in Battle.b.soulObjects)
                        {
                            s.changeCooldownColor();
                        }
                    }
                    else
                    {
                        Debug.Log("Enemy has been stunned");
                    }
                }
            }
        }
    }
}
