using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticles : MonoBehaviour
{
    float timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
