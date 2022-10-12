using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    Vector3 prevMousePosition = new Vector3(0, 0);
    public Grid grid;
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;        //this should be a list of placed blocks in grid
    public BattleState bs;
    public static Battle b;
    public List<Enemy> enemies;

    SoulObject selectedSoulObject;
    float selectedTime = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        b = this;
        bs = BattleState.PlayerGrid;
    }

    // Update is called once per frame
    void Update()
    {
        if (bs.Equals(BattleState.PlayerGrid)) {
            GridFitting();
        } else if (bs.Equals(BattleState.PlayerAction)) {
            PlayerTurn();
        } else if (bs.Equals(BattleState.EnemyAction)) {
            EnemyTurn();
        }
    }

    void PlayerTurn() {
        //take the list of soul blocks placed in the grid
        foreach (SoulObject soulObject in placedSoulObjects) {
            //attack animation
            PlayerSequence(soulObject);
        }
        placedSoulObjects.Clear();
        bs = BattleState.EnemyAction;
        Debug.Log("Enemy Turn");
    }

    void EnemyTurn() {
        foreach (Enemy e in enemies) {
            //attack animation
            EnemySequence(e);
        }
        bs = BattleState.PlayerGrid;
        Debug.Log("Grid Fitting");
    }

    void PlayerSequence(SoulObject obj) {
        //change later to add frames
        if (obj is SoulBlock)
        {
            //after single target, where do we save the attack enemy?
            SoulBlock s = (SoulBlock) obj;
            if (s.isAoe)
            {
                foreach (Enemy e in enemies)
                {
                    e.health -= s.damage;
                }
            }
            else if (s.isSingleTarget)
            {
                //s.getEnemy().health -= s.damage; //this getEnemy does not work
            }
            else if (s.isHeal)
            {
                Player.player.health += s.heal;
                if (Player.player.health > Player.player.MaxHealth)
                { //this number shouldn't be hard coded
                    Player.player.health = Player.player.MaxHealth;
                }
            }
        }
    }

    void EnemySequence(Enemy e) {
        // do smt
        Random rand = new Random();
        int i = rand.Next(0, 1);
        Player.player.health -= e.attack[i];
    }

    void GridFitting() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (selectedSoulObject != null)
        {
            if (selectedTime > 0)
            {
                selectedSoulObject.transform.position += mousePosition - prevMousePosition;
            }
            else if (selectedTime > float.MinValue)
            {
                selectedTime = float.MinValue;
                placeSoulObject(selectedSoulObject);
                selectedSoulObject = null;
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (selectedSoulObject == null)
            {
                foreach (SoulObject soulObject in soulObjects)
                {
                    if (!soulObject.placed)
                    {
                        if (soulObject.mouseTouching)
                        {
                            selectedSoulObject = soulObject;
                            if (soulObject is SoulBlock)
                            {
                                selectedSoulObject.soulRenderer.sortingOrder = 10;
                            }
                            else
                            {
                                selectedSoulObject.soulRenderer.sortingOrder = 4;
                            }
                            selectedTime = 0.05f;
                        }
                    }
                }
            }
        }
        else
        {
            selectedTime -= Time.deltaTime;
        }
        prevMousePosition = mousePosition;
    }


    void placeSoulObject(SoulObject soulObject)
    {
        List<Tile> touchingTiles = new List<Tile>();
        foreach (Tile t in grid.tiles)
        {
            if (soulObject is SoulBlock)
            {
                if (!t.filled && t.TouchingSoulObject(soulObject))
                {
                    touchingTiles.Add(t);
                }
            }
            else
            {
                if (!t.filled && !t.framed && t.TouchingSoulObject(soulObject))
                {
                    touchingTiles.Add(t);
                }
            }
        }

        if (touchingTiles.Count >= soulObject.squareCount)
        {
            if (soulObject is SoulBlock)
            {
                foreach (Tile tile in touchingTiles)
                {
                    tile.filled = true;
                }
            }
            else
            {
                foreach (Tile tile in touchingTiles)
                {
                    tile.framed = true;
                }
            }
            float refX = soulObject.transform.position.x + soulObject.relX;
            float refY = soulObject.transform.position.y + soulObject.relY;

            Tile t = closestTile(touchingTiles, refX, refY);

            soulObject.transform.position += new Vector3(t.transform.position.x - (refX), t.transform.position.y - (refY), 0);

            soulObject.placed = true;
            if (soulObject is SoulBlock)
            {
                soulObject.soulRenderer.sortingOrder = 5;
                updateFrames();
            }
            else
            {
                soulObject.soulRenderer.sortingOrder = 2;
            }

            grid.soulObjectsInGrid.Add(soulObject);
        }
        else
        {
            if (soulObject is SoulBlock)
            {
                soulObject.soulRenderer.sortingOrder = 6;
            }
            else
            {
                soulObject.soulRenderer.sortingOrder = 3;
            }
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

    void updateFrames()
    {
        foreach (SoulObject soulObject in grid.soulObjectsInGrid)
        {
            if (soulObject is SoulFrame)
            {
                SoulFrame soulFrame = (SoulFrame)soulObject;
                if (soulFrame.filled)
                {
                    continue;
                }
                int count = 0;
                foreach (Tile t in grid.tiles)
                {
                    if (t.filled && t.TouchingSoulObject(soulFrame))
                    {
                        count++;
                    }
                }
                if (count >= soulFrame.squareCount)
                {
                    soulFrame.filled = true;
                    soulFrame.soulRenderer.sortingOrder = 20;
                }
            }
        }
    }
}
