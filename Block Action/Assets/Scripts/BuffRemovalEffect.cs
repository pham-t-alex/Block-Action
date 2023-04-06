using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffRemovalEffect : Effect
{
    public int quantity;

    public BuffRemovalEffect(int quantity)
    {
        this.quantity = quantity;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int buffCountRemoved = 0;
                int i = f.statusEffects.Count - 1;
                while (buffCountRemoved < quantity && i >= 0)
                {
                    Status s = f.statusEffects[i];
                    if (s.getQuality() == Quality.Good && s.removable)
                    {
                        s.terminate();
                        f.statusEffects.RemoveAt(i);
                        buffCountRemoved++;
                    }
                    i--;
                }
                if (f.Equals(Player.player))
                {
                    Debug.Log(buffCountRemoved + " buffs removed from the player.");
                }
                else
                {
                    Debug.Log(buffCountRemoved + " buffs removed from an enemy.");
                }
            }
        }
    }
}
