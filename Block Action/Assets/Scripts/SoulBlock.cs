using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBlock : SoulObject
{
    //list of variables used for turn calculations
    public bool isAoe;
    public bool isSingleTarget;
    public bool isHeal;
    public int damage;
    public int heal;

    // Start is called before the first frame update
    void Start()
    {
        soulCollider = GetComponent<Collider2D>();
        //make sure at least 1 of these booleans are set
        isAoe = false;
        isSingleTarget = false;
        isHeal = false;
        //make sure damage or heal is set
        damage = 0;
        heal = 0;

        soulRenderer = GetComponent<SpriteRenderer>();
        soulRenderer.sortingOrder = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
