using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage;
    public Fighter user;
    public Fighter target;
    public Vector3 delta;
    public float lifetime;
    public float elapsedTime;
    void Start()
    {
        lifetime = 0.2f;
        elapsedTime = 0;
    }

    public void Init(int damage, Fighter user, Fighter target, float xOffset, float yOffset) //xOffset is 1 if right edge, -1 if left edge; yOffset is 1 if top, -1 if bottom
    {
        this.damage = damage;
        this.user = user;
        this.target = target;
        transform.position = user.transform.position;
        transform.position += new Vector3(0.5f * xOffset * user.GetComponent<SpriteRenderer>().sprite.bounds.size.x, 0.5f * yOffset * user.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
        transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan((transform.position.y - target.transform.position.y) / (transform.position.x - target.transform.position.x)));
        delta = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float time = Time.deltaTime;
            elapsedTime += time;
            transform.Translate((time / lifetime) * delta);
        }
        if (elapsedTime > lifetime)
        {
            GameObject particles = GameObject.Instantiate(Resources.Load<GameObject>("DamageParticles"), target.transform.position, Quaternion.identity);
            ParticleSystem.EmissionModule emission = particles.GetComponent<ParticleSystem>().emission;
            emission.rateOverTime = 400 * damage / target.maxHealth;
            target.health -= damage;
            GameObject indicator = Resources.Load<GameObject>("Indicator");
            GameObject g = GameObject.Instantiate(indicator, target.transform);
            g.GetComponent<Indicator>().FlyAway();
            TMP_Text text = g.GetComponent<TMP_Text>();
            text.color = new Color(1, 0, 0);
            text.text = damage + "";
            
            if (target.Equals(Player.player))
            {
                PlayerAnimator.SetTrigger("Hurt");
            }
            if (user != null)
            {
                ActionController.TriggerWhenHitEffects(target);
            }
            Destroy(gameObject);
        }
    }
}
