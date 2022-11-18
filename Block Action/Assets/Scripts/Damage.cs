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
                GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("DamageParticles"), f.transform.position, Quaternion.identity);
                ParticleSystem.EmissionModule emission = particles.GetComponent<ParticleSystem>().emission;
                emission.rateOverTime = 400 * ((int)(dmg * fighter.buff * f.defenseBuff)) / f.maxHealth;
                f.health -= (int)(dmg * fighter.buff * f.defenseBuff);
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
}
