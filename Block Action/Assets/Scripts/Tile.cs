using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //Whether the tile is covered by a block
    public bool filled;
    //Whether the tile has a frame on it
    public bool framed;
    //If not locked, = 0
    public int lockDuration;
    // Start is called before the first frame update
    public bool locked
    {
        get
        {
            if (lockDuration > 0)
            {
                return true;
            }
            return false;
        }
    }
    void Start()
    {
        filled = false;
        framed = false;
        lockDuration = 0;
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

    public void lockTile(int duration)
    {
        GridFitter.gridFitter.grid.lockedTiles.Add(gameObject);
        lockDuration += duration;
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
    }

    public void decrementLock()
    {
        if (lockDuration > 0)
        {
            lockDuration--;
        }
        if (lockDuration <= 0)
        {
            lockDuration = 0;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }
}
