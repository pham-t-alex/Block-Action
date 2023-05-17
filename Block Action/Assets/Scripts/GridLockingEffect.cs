using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLockingEffect : Effect
{
    public int count;
    public int duration;
    public GridLockingEffect(int count, int duration)
    {
        this.count = count;
        this.duration = duration;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        Grid g = GridFitter.gridFitter.grid;
        if (count > g.tiles.Count)
        {
            foreach (GameObject t in g.tiles)
            {
                t.GetComponent<Tile>().lockTile(duration);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, g.tiles.Count - 1);
                g.tiles[index].GetComponent<Tile>().lockTile(duration);
            }
        }
        Debug.Log("Tiles locked.");
    }
}
