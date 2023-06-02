using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeHand : MonoBehaviour
{
    public static TreeHand treeHand;
    public Sprite treeHandIdle;
    public Sprite treeHandAttack;
    public float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        treeHand = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                GetComponent<SpriteRenderer>().sprite = treeHandIdle;
                GetComponent<SpriteRenderer>().sortingOrder = -200;
                transform.position -= new Vector3(1.5f, 1.1f);
            }
        }
    }

    public void DealDamage(int damage, Fighter user)
    {
        GetComponent<SpriteRenderer>().sprite = treeHandAttack;
        GetComponent<SpriteRenderer>().sortingOrder = -160;
        transform.position += new Vector3(1.5f, 1.1f);
        time = 0.3f;

        GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("DamageParticles"), Player.player.transform.position, Quaternion.identity);
        ParticleSystem.EmissionModule emission = particles.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 400 * damage / Player.player.maxHealth;
        Player.player.health -= damage;
        GameObject indicator = Resources.Load<GameObject>("Indicator");
        GameObject g = GameObject.Instantiate(indicator, Player.player.transform);
        g.GetComponent<Indicator>().FlyAway();
        TMP_Text text = g.GetComponent<TMP_Text>();
        text.color = new Color(1, 0, 0);
        text.text = damage + "";

        PlayerAnimator.SetTrigger("Hurt");
        if (user != null)
        {
            ActionController.TriggerWhenHitEffects(Player.player);
            ActionController.TriggerLifeStealEffects(user, damage);
        }
        
    }
}
