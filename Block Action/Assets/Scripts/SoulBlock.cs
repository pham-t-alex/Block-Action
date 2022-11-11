using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulBlock : SoulObject
{
    //list of variables used for turn calculations
    

    // Start is called before the first frame update
    void Start()
    {
        soulCollider = GetComponent<Collider2D>();

        soulRenderer = GetComponent<SpriteRenderer>();
        soulRenderer.sortingOrder = 6;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
