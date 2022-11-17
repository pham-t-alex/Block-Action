using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buff : Effect
{
    public double buff;

    public Buff(double buff) {
        this.buff = buff;
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
                if (buff < 0)
                {
                    GameObject healParticles = GameObject.Instantiate(Resources.Load<GameObject>("DebuffParticles"), f.transform.position, Quaternion.identity);
                    ParticleSystem.ShapeModule sm = healParticles.GetComponent<ParticleSystem>().shape;
                    sm.scale = new Vector3(f.GetComponent<SpriteRenderer>().bounds.size.x, f.GetComponent<SpriteRenderer>().bounds.size.y);
                }
                if (buff > 0)
                {
                    GameObject healParticles = GameObject.Instantiate(Resources.Load<GameObject>("BuffParticles"), f.transform.position, Quaternion.identity);
                    ParticleSystem.ShapeModule sm = healParticles.GetComponent<ParticleSystem>().shape;
                    sm.scale = new Vector3(f.GetComponent<SpriteRenderer>().bounds.size.x, f.GetComponent<SpriteRenderer>().bounds.size.y);
                }
                BuffCounter bc = new BuffCounter(numTurns, buff);
                f.buffLeft.Add(bc);
                double prevBuff = f.buff;
                f.buff += buff;
                if (f.Equals(Player.player))
                {
                    Debug.Log("Player buff set from " + prevBuff + "x to " + f.buff + "x");
                }
                else
                {
                    Debug.Log("Enemy buff set to " + prevBuff + "x to " + f.buff + "x");
                }
            }
        }
    }
}
