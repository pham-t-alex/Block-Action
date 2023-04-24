using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrueDamage : Effect
{

    public double dmg;

    public TrueDamage(int dmg)
    {
        this.dmg = dmg;
    }


    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int prevHealth = f.health;
                f.health -= (int)(dmg);
                GameObject indicator = Resources.Load<GameObject>("Indicator");
                GameObject g = GameObject.Instantiate(indicator, f.transform);
                TMP_Text text = g.GetComponent<TMP_Text>();
                text.color = new Color(1, 0, 0);
                text.text = (int)(dmg) + "";

                if (f.Equals(Player.player))
                {
                    // Hurt animation only plays when Player takes damage
                    PlayerAnimator.SetTrigger("Hurt");
                    Debug.Log("Player takes " + (int)(dmg) + " true damage | HP: " + (prevHealth) + " -> " + f.health);
                }
                else
                {
                    Debug.Log("Enemy takes " + (int)(dmg) + " true damage | HP: " + (prevHealth) + " -> " + f.health);
                }

                if (fighter != null)
                {
                    ActionController.TriggerWhenHitEffects(f);
                }
            }
        }
        if (fighter != null)
        {
            ActionController.TriggerAfterDamageEffects(fighter);
        }
    }
}
