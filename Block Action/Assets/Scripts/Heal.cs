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
            if (!f.dead)
            {
                GameObject healParticles = GameObject.Instantiate(Resources.Load<GameObject>("HealParticles"), f.transform.position, Quaternion.identity);
                healParticles.transform.position -= new Vector3(0, f.GetComponent<SpriteRenderer>().bounds.size.y / 2);
                ParticleSystem.ShapeModule sm = healParticles.GetComponent<ParticleSystem>().shape;
                sm.radius = (f.GetComponent<SpriteRenderer>().bounds.size.x / 2);
                if (f.health < f.maxHealth)
                {
                    int prevHealth = f.health;
                    f.health += (int)heal;
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
