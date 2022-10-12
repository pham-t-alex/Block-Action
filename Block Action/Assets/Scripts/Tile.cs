using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool filled;
    // Start is called before the first frame update
    void Start()
    {
        filled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TouchingBlock(SoulBlock block)
    {
        if (block.blockCollider.OverlapPoint(new Vector2(transform.position.x, transform.position.y)))
        {
            return true;
        }
        return false;
    }
}
