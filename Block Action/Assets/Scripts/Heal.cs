using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Heal : Effect
{

    public double heal;

    public Heal(double heal)
    {
        this.heal = heal;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                if (f.health < f.maxHealth)
                {
                    int prevHealth = f.health;
                    f.health += (int)heal;
                    GameObject indicator = Resources.Load<GameObject>("Indicator");
                    GameObject g = GameObject.Instantiate(indicator, f.transform);
                    TMP_Text text = g.GetComponent<TMP_Text>();
                    text.color = new Color(0, 1, 0);
                    text.text = "" + (int)heal;

                    if (f.health > f.maxHealth)
                    {
                        f.health = f.maxHealth;
                    }
                    if (f.Equals(Player.player))
                    {
                        Debug.Log("Player heals " + heal + " health | HP: " + prevHealth + " -> " + f.health);
                    }
                    else
                    {
                        Debug.Log("Enemy heals " + heal + " health | HP: " + prevHealth + " -> " + f.health);
                    }
                }
                else
                {
                    if (f.Equals(Player.player))
                    {
                        Debug.Log("Player is at max health! | HP: " + f.health);
                    }
                    else
                    {
                        Debug.Log("Enemy is at max health! | HP: " + f.health);
                    }
                }
            }
        }
    }

    void SetHeal(double heal)
    {
        this.heal = heal;
    }
}
