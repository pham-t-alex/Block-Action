using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrueDamage : Effect
{

    public double dmg;

    public TrueDamage(int dmg)
    {
        this.dmg = dmg;
    }


    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                int prevHealth = f.health;
                if (fighter is Enemy e)
                {
                    if (e.type == "Lizard_Monster")
                    {
                        AttackProjectile p = GameObject.Instantiate(Resources.Load<GameObject>("Fireball")).GetComponent<AttackProjectile>();
                        p.Init((int)dmg, fighter, f, -0.5f, 0.5f);
                        continue;
                    }
                    else if (e.type == "Will_o_Wisp")
                    {
                        AttackProjectile p = GameObject.Instantiate(Resources.Load<GameObject>("Wispball")).GetComponent<AttackProjectile>();
                        p.Init((int)dmg, fighter, f, 0, 0.5f);
                        continue;
                    }
                    else if (e.type == "Big_Tree" && targetType == TargetType.SingleTarget)
                    {
                        TreeHand.treeHand.DealDamage((int)dmg, fighter);
                        continue;
                    }
                }
                f.health -= (int)(dmg);
                if (fighter != null)
                {
                    GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("DamageParticles"), f.transform.position, Quaternion.identity);
                    ParticleSystem.EmissionModule emission = particles.GetComponent<ParticleSystem>().emission;
                    emission.rateOverTime = 400 * (int) dmg / f.maxHealth;
                }
                GameObject indicator = Resources.Load<GameObject>("Indicator");
                GameObject g = GameObject.Instantiate(indicator, f.transform);
                g.GetComponent<Indicator>().FlyAway();
                TMP_Text text = g.GetComponent<TMP_Text>();
                text.color = new Color(1, 0, 0);
                text.text = (int)(dmg) + "";

                if (f.Equals(Player.player))
                {
                    // Hurt animation only plays when Player takes damage
                    PlayerAnimator.SetTrigger("Hurt");
                    Debug.Log("Player takes " + (int)(dmg) + " true damage | HP: " + (prevHealth) + " -> " + f.health);
                }
                else
                {
                    Debug.Log("Enemy takes " + (int)(dmg) + " true damage | HP: " + (prevHealth) + " -> " + f.health);
                }

                if (fighter != null)
                {
                    ActionController.TriggerWhenHitEffects(f);
                    ActionController.TriggerLifeStealEffects(fighter, (int)dmg);
                }
            }
        }
        if (fighter != null)
        {
            ActionController.TriggerAfterDamageEffects(fighter);
        }
    }
}
