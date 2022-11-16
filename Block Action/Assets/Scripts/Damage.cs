using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : Effect
{

    public double dmg;

    public Damage(int dmg) {
        this.dmg = dmg;
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
        foreach (Fighter f in targets) {
            if (!f.dead)
            {
                int prevHealth = f.health;
                f.health -= (int)(dmg * fighter.buff * f.defenseBuff);
                if (f.Equals(Player.player))
                {
                    PlayerAnimator.SetTrigger("Hurt");
                    Debug.Log("Player takes " + (dmg * fighter.buff * f.defenseBuff) + " damage | HP: " + (prevHealth) + " -> " + f.health);
                }
                else
                {
                    Debug.Log("Enemy takes " + (dmg * fighter.buff * f.defenseBuff) + " damage | HP: " + (prevHealth) + " -> " + f.health);
                }
            }
        }
    }
}
