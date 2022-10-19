using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{
    public List<int> attack; // change to effect
    public bool mouseTouching;

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

    void OnMouseEnter()
    {
        mouseTouching = true;
    }

    private void OnMouseExit()
    {
        mouseTouching = false;
    }
}
