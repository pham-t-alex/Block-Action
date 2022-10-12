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

        foreach (SoulBlock block in soulBlocks)
        {
            if (!block.placed)
            {
                if (Input.GetMouseButton(0) && block.mouseTouching)
                {
                    block.selectedTime = 0.05f;
                }
                else
                {
                    block.selectedTime -= Time.deltaTime;
                }
                if (block.selectedTime < 0)
                {
                    block.selectedTime = 0;
                    placeBlock(block);
                }
                if (block.selectedTime > 0)
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
        List<Tile> touchingTiles = new List<Tile>();
        foreach (Tile t in grid.tiles)
        {
            if(!t.filled && t.TouchingBlock(block))
            {
                touchingTiles.Add(t);
            }
        }

        if (touchingTiles.Count >= block.squareCount)
        {
            foreach (Tile tile in touchingTiles)
            {
                tile.filled = true;
            }
            float refX = block.transform.position.x + block.relX;
            float refY = block.transform.position.y + block.relY;

            Tile t = closestTile(touchingTiles, refX, refY);

            block.transform.position += new Vector3(t.transform.position.x - (refX), t.transform.position.y - (refY), 0);

            block.placed = true;
        }
    }

    Tile closestTile(List<Tile> tiles, float refX, float refY)
    {
        Tile closest = tiles[0];
        float minDist = distanceBetween(closest, refX, refY);
        foreach (Tile t in tiles)
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

    float distanceBetween(Tile t, float refX, float refY)
    {
        return Vector3.Distance(t.transform.position - new Vector3(0, 0, t.transform.position.z), new Vector3(refX, refY, 0));
    }
}
