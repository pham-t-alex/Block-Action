using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Effect
{

    public double heal;

    public Heal(double heal)
    {
        this.heal = heal;
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
            if (f.health < f.maxHealth)
            {
                f.health += (int)heal;
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player heals " + heal + " health | HP: " + (f.health - heal) + " -> " + f.health);
                }
                else
                {
                    Debug.Log("Enemy heals " + heal + " health | HP: " + (f.health - heal) + " -> " + f.health);
                }
            }
            else {
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

    void SetHeal(double heal)
    {
        this.heal = heal;
    }
}
