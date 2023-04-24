using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefIgnoringDamage : Effect
{

    public double dmg;
    public Element.Elements element;

    public DefIgnoringDamage(int dmg)
    {
        this.dmg = dmg;
    }


    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int damageDealt;
                int prevHealth = f.health;
                if (fighter != null)
                {
                    damageDealt = (int)(dmg * makeNonNegative(fighter.buff) * Element.elementalDamageModifier(element, f.currentElement));
                    f.health -= damageDealt;
                }
                else
                {
                    damageDealt = (int)(dmg * Element.elementalDamageModifier(element, f.currentElement));
                    f.health -= damageDealt;
                }
                GameObject indicator = Resources.Load<GameObject>("Indicator");
                GameObject g = GameObject.Instantiate(indicator, f.transform);
                TMP_Text text = g.GetComponent<TMP_Text>();
                text.color = new Color(1, 0, 0);
                text.text = damageDealt + "";

                if (f.Equals(Player.player))
                {
                    // Hurt animation only plays when Player takes damage
                    PlayerAnimator.SetTrigger("Hurt");
                    Debug.Log("Player takes " + damageDealt + " def-ignoring damage | HP: " + (prevHealth) + " -> " + f.health);
                }
                else
                {
                    Debug.Log("Enemy takes " + damageDealt + " def-ignoring damage | HP: " + (prevHealth) + " -> " + f.health);
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

    private double makeNonNegative(double multiplier)
    {
        if (multiplier < 0)
        {
            return 0;
        }
        return multiplier;
    }
}
