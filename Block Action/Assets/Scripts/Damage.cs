using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            f.health -= (int) (dmg * fighter.buff);
            if (f.Equals(Player.player))
            {
                Debug.Log("Player takes " + (dmg * fighter.buff) + " damage | HP: " + (f.health + (dmg * fighter.buff)) + " -> " + f.health);
            }
            else {
                Debug.Log("Enemy takes " + (dmg * fighter.buff) + " damage | HP: " + (f.health + (dmg * fighter.buff)) + " -> " + f.health);
            }
        }
    }
}
