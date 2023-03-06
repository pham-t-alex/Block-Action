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
                f.health -= (int)(dmg * makeNonNegative(fighter.buff) * makeNonNegative(f.defenseBuff));
                GameObject indicator = Resources.Load<GameObject>("Indicator");
                GameObject g = GameObject.Instantiate(indicator, f.transform);
                TMP_Text text = g.GetComponent<TMP_Text>();
                text.color = new Color(1, 0, 0);
                text.text = (int)(dmg * fighter.buff * f.defenseBuff) + "";

                if (f.Equals(Player.player))
                {
                    // Hurt animation only plays when Player takes damage
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

    private double makeNonNegative(double multiplier)
    {
        if (multiplier < 0)
        {
            return 0;
        }
        return multiplier;
    }
}
