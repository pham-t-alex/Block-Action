using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private static ParticleHandler _particleHandler;
    public static ParticleHandler particleHandler {
        get
        {
            if (_particleHandler == null)
            {
                _particleHandler = FindObjectOfType<ParticleHandler>();
            }
            return _particleHandler;
        }
    }
    public GameObject soulParticle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static SoulParticle CreateSoulParticle(Vector3 position, Color c)
    {
        SoulParticle sp = Instantiate(particleHandler.soulParticle, position, Quaternion.identity).GetComponent<SoulParticle>();
        sp.SetColor(c);
        return sp;
    }
}
