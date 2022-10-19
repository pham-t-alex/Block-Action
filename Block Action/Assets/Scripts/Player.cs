using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{ 
    public int MaxHealth;
    public static Player player;

    // Start is called before the first frame update
    void Start()
    {
        health = 100; //might not be valid
        MaxHealth = 100;
        player = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
