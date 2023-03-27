using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffRemovalEffect : Effect
{
    public int quantity;

    public DebuffRemovalEffect(int quantity)
    {
        this.quantity = quantity;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int debuffCountRemoved = 0;
                int i = f.statusEffects.Count - 1;
                while (debuffCountRemoved < quantity && i >= 0)
                {
                    Status s = f.statusEffects[i];
                    if (s.getQuality() == Quality.Bad && s.removable)
                    {
                        s.terminate();
                        f.statusEffects.RemoveAt(i);
                        debuffCountRemoved++;
                    }
                    i--;
                }
                if (f.Equals(Player.player))
                {
                    Debug.Log(debuffCountRemoved + " debuffs removed from the player.");
                }
                else
                {
                    Debug.Log(debuffCountRemoved + " debuffs removed from an enemy.");
                }
            }
        }
    }
}
