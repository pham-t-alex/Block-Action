using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    Vector3 prevMousePosition = new Vector3(0, 0);
    public Grid grid;
    public List<SoulBlock> soulBlocks;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        foreach (SoulBlock block in soulBlocks) // iterates through all available soul blocks
        {
            if (!block.placed) // when the block has not been placed down
            {
                if (Input.GetMouseButton(0) && block.mouseTouching) // if mouse held down on block
                {
                    block.selectedTime = 0.05f; // amt of time it is stuck to mouse
                }
                else // if mouse isn't held down on block
                {
                    block.selectedTime -= Time.deltaTime;
                }
                if (block.selectedTime < 0) // place down block
                {
                    block.selectedTime = 0;
                    placeBlock(block);
                }
                if (block.selectedTime > 0) // move mouse towards block
                {
                    block.transform.position += mousePosition - prevMousePosition;
                }
            }
        }
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        prevMousePosition = mousePosition;
    }
    void placeBlock(SoulBlock block)
    {
        List<GameObject> touchingTiles = new List<GameObject>(); // creation of array list
        foreach (GameObject t in grid.tiles) // test if block is touching all tiles
        {
            if(!t.GetComponent<Tile>().filled && t.GetComponent<Tile>().TouchingBlock(block))
            {
                touchingTiles.Add(t);
            }
        }

        if (touchingTiles.Count >= block.squareCount)
        // are all of the tiles touching the correct amount of squares?
        {
            foreach (GameObject tile in touchingTiles)
            {
                tile.GetComponent<Tile>().filled = true;
                //sets all tiles in array list so that they are filed by the block
            }
            float refX = block.transform.position.x + block.relX;
            float refY = block.transform.position.y + block.relY;
            // sets reference points for block

            GameObject t = closestTile(touchingTiles, refX, refY);

            block.transform.position += new Vector3(t.transform.position.x - (refX), t.transform.position.y - (refY), 0);
            // snaps blocks to closest tile.

            block.placed = true;
        }
    }

    GameObject closestTile(List<GameObject> tiles, float refX, float refY)
    {
        GameObject closest = tiles[0];
        float minDist = distanceBetween(closest, refX, refY);
        foreach (GameObject t in tiles)
        {
            float dist = distanceBetween(t, refX, refY);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }

    float distanceBetween(GameObject t, float refX, float refY)
    {
        return Vector3.Distance(t.transform.position - new Vector3(0, 0, t.transform.position.z), new Vector3(refX, refY, 0));
    }
}
