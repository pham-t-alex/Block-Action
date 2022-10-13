using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public List<int> attack; // change to effect

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        Battle.b.enemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
