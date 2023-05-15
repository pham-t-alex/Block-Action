using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUserParticle : MonoBehaviour
{
    private static ActionUserParticle _actionUserParticle;
    public static ActionUserParticle actionUserParticle
    {
        get
        {
            if (_actionUserParticle == null)
            {
                _actionUserParticle = FindObjectOfType<ActionUserParticle>();
            }
            return _actionUserParticle;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _actionUserParticle = this;
        disable();
    }

    public void disable()
    {
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.rateOverTime = 0;
    }

    public void setAction(Fighter f)
    {
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.rateOverTime = 20;
        transform.position = f.transform.position - new Vector3(0, f.GetComponent<SpriteRenderer>().bounds.size.y / 2f);
        ParticleSystem.ShapeModule sm = GetComponent<ParticleSystem>().shape;
        sm.radius = (f.GetComponent<SpriteRenderer>().bounds.size.x / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
