using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : Effect
{

    public double dmg;
    public Element.Elements element;

    public Damage(int dmg)
    {
        this.dmg = dmg;
    }


    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets) {
            if (!f.dead)
            {
                int damageDealt;
                int prevHealth = f.health;
                if (fighter != null)
                {
                    damageDealt = (int)(dmg * makeNonNegative(fighter.buff) * makeNonNegative(f.defenseBuff) * Element.elementalDamageModifier(element, f.currentElement));
                    if (fighter is Enemy e)
                    {
                        if (e.type == "Lizard_Monster")
                        {
                            AttackProjectile p = GameObject.Instantiate(Resources.Load<GameObject>("Fireball")).GetComponent<AttackProjectile>();
                            p.Init(damageDealt, fighter, f, -0.5f, 0.5f);
                            continue;
                        }
                        else if (e.type == "Will_o_Wisp")
                        {
                            AttackProjectile p = GameObject.Instantiate(Resources.Load<GameObject>("Wispball")).GetComponent<AttackProjectile>();
                            p.Init(damageDealt, fighter, f, 0, 0.5f);
                            continue;
                        }
                        else if (e.type == "Big_Tree")
                        {
                            TreeHand.treeHand.DealDamage(damageDealt, fighter);
                            continue;
                        }
                    }
                    GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("DamageParticles"), f.transform.position, Quaternion.identity);
                    ParticleSystem.EmissionModule emission = particles.GetComponent<ParticleSystem>().emission;
                    emission.rateOverTime = 400 * damageDealt / f.maxHealth;
                    f.health -= damageDealt;
                }
                else
                {
                    damageDealt = (int)(dmg * makeNonNegative(f.defenseBuff) * Element.elementalDamageModifier(element, f.currentElement));
                    f.health -= damageDealt;
                }
                GameObject indicator = Resources.Load<GameObject>("Indicator");
                GameObject g = GameObject.Instantiate(indicator, f.transform);
                g.GetComponent<Indicator>().FlyAway();
                TMP_Text text = g.GetComponent<TMP_Text>();
                text.color = new Color(1, 0, 0);
                text.text = damageDealt + "";

                if (f.Equals(Player.player))
                {
                    // Hurt animation only plays when Player takes damage
                    PlayerAnimator.SetTrigger("Hurt");
                    Debug.Log("Player takes " + damageDealt + " damage | HP: " + (prevHealth) + " -> " + f.health);
                }
                else
                {
                    //EnemyAnimator.SetTrigger("Hurt");
                    Animator a = f.GetComponent<Animator>();
                    if (a != null)
                    {
                        a.SetTrigger("Hurt");
                    }
                    Debug.Log("Enemy takes " + damageDealt + " damage | HP: " + (prevHealth) + " -> " + f.health);
                }

                if (fighter != null)
                {
                    ActionController.TriggerWhenHitEffects(f);
                    ActionController.TriggerLifeStealEffects(fighter, damageDealt);
                }
            }
        }
        if (fighter != null)
        {
            ActionController.TriggerAfterDamageEffects(fighter);
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
