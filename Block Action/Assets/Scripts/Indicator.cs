using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public float timer;
    public float direction;
    public float vel;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
    }

    public void FlyAway()
    {
        direction = Random.Range(45f, 135f);
        vel = Random.Range(4.0f, 5.0f);
    }

    public void Offset()
    {
        transform.Translate(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            float elapsedTime = Time.deltaTime;
            timer -= elapsedTime;
            if (vel > 0)
            {
                transform.Translate(elapsedTime * vel * timer * Mathf.Cos(Mathf.Deg2Rad * direction), elapsedTime * vel * timer * Mathf.Sin(Mathf.Deg2Rad * direction), 0);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
