using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Whether the tile is covered by a block
    public bool filled;
    //Whether the tile has a frame on it
    public bool framed;
    // Start is called before the first frame update
    void Start()
    {
        filled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TouchingSoulObject(SoulObject soulObject)
    {
        if (soulObject.soulCollider.OverlapPoint(new Vector2(transform.position.x, transform.position.y)))
        {
            return true;
        }
        return false;
    }
}
