using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public List<int> attack;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        attack = new List<int>();
        attack.Add(10);
        attack.Add(20);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
