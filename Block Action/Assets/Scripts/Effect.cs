using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public bool self;
    public List<Fighter> targets = new List<Fighter>();
    public int numTurns;    //for buffing

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void ActivateEffect(Fighter fighter);
}
