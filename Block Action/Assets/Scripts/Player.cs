using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private static Player _player;
    public static Player player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
            }
            return _player;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 100; //might not be valid
        buff = 1.0;
        maxHealth = 100;
        buffLeft = new List<BuffCounter>();

        Battle.b.fighters.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
