using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Effect
{

    public int heal;

    public Heal(int heal)
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

    public override void ActivateEffect(List<Fighter> t)
    {
        foreach (Fighter f in t)
        {
            f.health += heal;
            Debug.Log("Player heals " + heal + " health | HP: " + (f.health - heal) + " -> " + f.health);
        }
    }

    void SetHeal(int heal)
    {
        this.heal = heal;
    }
}
