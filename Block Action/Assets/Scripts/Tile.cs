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
    public int maxDuration;
    // Start is called before the first frame update
    public GameObject tileLock;
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
        maxDuration = 0;
        tileLock = null;
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
        if (lockDuration <= 0)
        {
            maxDuration = duration;
            lockDuration = duration;
            GridFitter.gridFitter.grid.lockedTiles.Add(gameObject);
            tileLock = Instantiate(Resources.Load<GameObject>("TileLock"));
            tileLock.transform.position = transform.position;
            tileLock.transform.localScale = new Vector3(GridFitter.gridFitter.scale, GridFitter.gridFitter.scale);
            GetComponent<SpriteRenderer>().color = new Color(82f/255, 96f/255, 82f/255);
        }
        else
        {
            if (duration > lockDuration)
            {
                maxDuration = duration;
                lockDuration = duration;
            }
        }
    }

    public void decrementLock()
    {
        if (lockDuration > 0)
        {
            lockDuration--;
            GetComponent<SpriteRenderer>().color = new Color((36f + ((maxDuration - lockDuration) / (float)maxDuration) * (255-36))/255, (45f + ((maxDuration - lockDuration) / (float)maxDuration) * (255 - 45)) /255, (36f + ((maxDuration - lockDuration) / (float)maxDuration) * (255 - 36)) /255);
        }
        if (lockDuration <= 0)
        {
            lockDuration = 0;
            Destroy(tileLock);
            tileLock = null;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }
}
