using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Effect
{

    public int dmg;

    public Damage(int dmg) {
        this.dmg = dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ActivateEffect(List<Fighter> t)
    {
        foreach (Fighter f in t) {
            f.health -= dmg;
            Debug.Log(f.health);
        }
    }
}
