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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (filled)
        {
            SetActiveParticles(true);
        }
        else
        {
            SetActiveParticles(false);
        }
    }
}
