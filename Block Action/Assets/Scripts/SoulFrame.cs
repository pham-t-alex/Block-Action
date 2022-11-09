using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulFrame : SoulObject
{
    //Whether the frame is filled with SoulBlocks
    public bool filled;
    // Start is called before the first frame update
    void Start()
    {
        soulCollider = GetComponent<Collider2D>();
        soulRenderer = GetComponent<SpriteRenderer>();
        soulRenderer.sortingOrder = 3;
        filled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
