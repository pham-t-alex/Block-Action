using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{ 
    public static Player player;

    // Start is called before the first frame update
    void Start()
    {
        health = 100; //might not be valid
        buff = 1.0;
        maxHealth = 100;
        player = this;
        buffLeft = new List<BuffCounter>();

        Battle.b.fighters.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
