using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : Effect
{
    public double dmg;
    public GameObject text = GameObject.Find("Output");

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
                // Change this back to Debug.Log after plz
                text.GetComponent<TMP_Text>().text += "Player takes " + (dmg * fighter.buff) + " damage | HP: " + (f.health + (dmg * fighter.buff)) + " -> " + f.health + "\n";
            }
            else {
                text.GetComponent<TMP_Text>().text += "Enemy takes " + (dmg * fighter.buff) + " damage | HP: " + (f.health + (dmg * fighter.buff)) + " -> " + f.health +"\n";
            }
        }
    }
}
