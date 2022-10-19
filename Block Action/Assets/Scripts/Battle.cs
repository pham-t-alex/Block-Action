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
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    public static Battle b;
    public List<Enemy> enemies;
    public GameObject target;

    SoulObject selectedSoulObject;
    float selectedTime = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        b = this;
        bs = BattleState.PlayerGrid;
        Effect e1 = new Damage(50);
        Effect e2 = new Heal(20);
        Effect e3 = new Damage(5);
        e1.self = false;
        e2.self = true;
        e3.self = false;
        soulObjects[0].effects.Add(e1);
        soulObjects[1].effects.Add(e2);
        soulObjects[2].effects.Add(e3);
        target.GetComponent<SpriteRenderer>().sortingOrder = 100;
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
        } else if (bs.Equals(BattleState.EnemySelect)) {
            EnemySelect();
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
        //reset soulblocks to original position
        Debug.Log("Grid Fitting");
    }

    void PlayerSequence(SoulObject s) {
        //change later to add frames
        //after single target, where do we save the attack enemy?
        s.ActivateEffect();
        foreach (Enemy e in enemies)
        {
            if (e.health <= 0) {
                e.gameObject.SetActive(false);
            }
        }
    }

    //revamp this
    void EnemySequence(Enemy e) {
        // do smt
        Random rand = new Random();
        int i = rand.Next(0, 2);
        Player.player.health -= e.attack[i];
        Debug.Log(Player.player.health);
        Player.player.gameObject.SetActive(false);
    }


    void GridFitting() {
        target.SetActive(false);
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

    void EnemySelect()
    {
        target.SetActive(true);
        bool touchingEnemy = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.mouseTouching)
            {
                touchingEnemy = true;
                if (Input.GetMouseButtonDown(0))
                {
                    selectedSoulObject.targets.Add(enemy);
                    selectedSoulObject = null;
                    bs = BattleState.PlayerGrid;
                }
                else
                {
                    target.transform.position = enemy.transform.position;
                }
                break;
            }
        }
        if (!touchingEnemy)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            target.transform.position = mousePosition;
        }
    }

    void placeSoulObject(SoulObject soulObject)
    {
        List<GameObject> touchingTiles = new List<GameObject>(); // creation of array list
        foreach (GameObject t in grid.tiles) // test if block is touching all tiles
        {
            if (soulObject is SoulBlock)
            {
                if (!t.GetComponent<Tile>().filled && t.GetComponent<Tile>().TouchingSoulObject(soulObject))
                {
                    touchingTiles.Add(t);
                }
            }
            else
            {
                if (!t.GetComponent<Tile>().filled && !t.GetComponent<Tile>().framed && t.GetComponent<Tile>().TouchingSoulObject(soulObject))
                {
                    touchingTiles.Add(t);
                }
            }
        }

        if (touchingTiles.Count >= soulObject.squareCount)
        {
            if (soulObject is SoulBlock)
            {
                foreach (GameObject tile in touchingTiles)
                {
                    tile.GetComponent<Tile>().filled = true;
                }
            }
            else
            {
                foreach (GameObject tile in touchingTiles)
                {
                    tile.GetComponent<Tile>().framed = true;
                }
            }
            float refX = soulObject.transform.position.x + soulObject.relX;
            float refY = soulObject.transform.position.y + soulObject.relY;

            GameObject t = closestTile(touchingTiles, refX, refY);

            soulObject.transform.position += new Vector3(t.transform.position.x - (refX), t.transform.position.y - (refY), 0);

            soulObject.placed = true;
            if (soulObject is SoulBlock)
            {
                soulObject.soulRenderer.sortingOrder = 5;
                placedSoulObjects.Add(soulObject);
                updateFrames();
            }
            else
            {
                soulObject.soulRenderer.sortingOrder = 2;
            }

            grid.soulObjectsInGrid.Add(soulObject);
            if (soulObject.isAoe)
            {
                foreach (Enemy e in enemies)
                {
                    soulObject.targets.Add(e);
                }
                selectedSoulObject = null;
            }
            else if (soulObject.isSingleTarget)
            {
                bs = BattleState.EnemySelect;
            }
        }
        else
        {
            selectedSoulObject = null;
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
                foreach (GameObject t in grid.tiles)
                {
                    if (t.GetComponent<Tile>().filled && t.GetComponent<Tile>().TouchingSoulObject(soulFrame))
                    {
                        count++;
                    }
                }
                if (count >= soulFrame.squareCount)
                {
                    soulFrame.filled = true;
                    soulFrame.soulRenderer.sortingOrder = 20;
                    placedSoulObjects.Add(soulObject);
                }
            }
        }
    }
}
