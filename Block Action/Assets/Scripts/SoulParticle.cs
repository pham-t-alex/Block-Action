using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulParticle : MonoBehaviour
{
    ParticleSystem ps
    {
        get
        {
            return GetComponent<ParticleSystem>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(Color c)
    {
        ParticleSystem.MainModule main = ps.main;
        c.a = 0;
        main.startColor = c;
    }

    public void IncreaseOpacity()
    {
        ParticleSystem.MainModule main = ps.main;
        Color c = main.startColor.color;
        c.a += 0.1f;
        main.startColor = c;
    }

    public IEnumerator FlyToPlayer()
    {
        if (Player.player.dead)
        {
            Destroy(gameObject);
        }
        Vector3 startPos = transform.position;
        Vector3 playerPos = Player.player.transform.position;
        Vector3 diff = playerPos - startPos;
        int max = 30;
        for (int i = 0; i < max; i++)
        {
            transform.position += diff / max;
            transform.localScale -= new Vector3(1.0f / max, 1.0f / max);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}
