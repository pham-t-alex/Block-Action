using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFitter : MonoBehaviour
{

    public static GridFitter gridFitter;

    public GameObject target; //target sprite gameobject
    static SoulObject selectedSoulObject; //currently selected soul object
    static float selectedTime = float.MinValue;
    static Vector3 prevMousePosition = new Vector3(0, 0); //the previous mouse position, to check delta mouse position
    static Vector3 prevObjectPosition = new Vector3(0, 0); //the previous object position
    public Grid grid;

    //For block placement
    public float leftOffset;
    public float inBetweenSpace;
    public float bottomOffset;
    public float scale;

    // Start is called before the first frame update
    private void Awake()
    {
        gridFitter = this;
    }
    void Start()
    {
        target.GetComponent<SpriteRenderer>().sortingOrder = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void GridFitting()
    {
        gridFitter.target.SetActive(false);
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
                foreach (SoulObject soulObject in Battle.b.soulObjects)
                {
                    if (!soulObject.placed)
                    {
                        if (soulObject.mouseTouching)
                        {
                            prevObjectPosition = soulObject.transform.position;
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

    static void placeSoulObject(SoulObject soulObject)
    {
        List<GameObject> touchingTiles = new List<GameObject>(); // creation of array list
        foreach (GameObject t in gridFitter.grid.tiles) // test if block is touching all tiles
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
                Battle.b.placedSoulObjects.Add(soulObject);
                updateFrames();
            }
            else
            {
                soulObject.soulRenderer.sortingOrder = 2;
            }

            gridFitter.grid.soulObjectsInGrid.Add(soulObject);
            if (soulObject.isAoe)
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    soulObject.targets.Add(e);
                }
                selectedSoulObject = null;
            }
            else if (soulObject.isSingleTarget)
            {
                Battle.b.bs = BattleState.EnemySelect;
            }
            else
            {
                selectedSoulObject = null;
            }
        }
        else
        {
            selectedSoulObject.transform.position = prevObjectPosition;
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

    static GameObject closestTile(List<GameObject> tiles, float refX, float refY)
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

    static float distanceBetween(GameObject t, float refX, float refY)
    {
        return Vector3.Distance(t.transform.position - new Vector3(0, 0, t.transform.position.z), new Vector3(refX, refY, 0));
    }

    static void updateFrames()
    {
        foreach (SoulObject soulObject in gridFitter.grid.soulObjectsInGrid)
        {
            if (soulObject is SoulFrame)
            {
                SoulFrame soulFrame = (SoulFrame)soulObject;
                if (soulFrame.filled)
                {
                    continue;
                }
                int count = 0;
                foreach (GameObject t in gridFitter.grid.tiles)
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
                    Battle.b.placedSoulObjects.Add(soulObject);
                }
            }
        }
    }

    public static void EnemySelect()
    {
        gridFitter.target.SetActive(true);
        bool touchingEnemy = false;
        foreach (Enemy enemy in Battle.b.enemies)
        {
            if (enemy.mouseTouching)
            {
                touchingEnemy = true;
                if (Input.GetMouseButtonDown(0))
                {
                    selectedSoulObject.targets.Add(enemy);
                    selectedSoulObject = null;
                    Battle.b.bs = BattleState.PlayerGrid;
                }
                else
                {
                    gridFitter.target.transform.position = enemy.transform.position;
                }
                break;
            }
        }
        if (!touchingEnemy)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            gridFitter.target.transform.position = mousePosition;
        }
    }

    public static void ResetSoulObjects()
    {
        foreach (SoulObject soulObject in Battle.b.placedSoulObjects)
        {
            foreach (GameObject t in gridFitter.grid.tiles)
            {
                if (t.GetComponent<Tile>().TouchingSoulObject(soulObject))
                {
                    if (soulObject is SoulBlock)
                    {
                        t.GetComponent<Tile>().filled = false;
                    }
                    else if (soulObject is SoulFrame)
                    {
                        t.GetComponent<Tile>().framed = false;
                    }
                }
            }
            soulObject.placed = false;
            if (soulObject is SoulFrame)
            {
                soulObject.soulRenderer.sortingOrder = 3;
                SoulFrame s = (SoulFrame)soulObject;
                s.filled = false;
            }
            else if (soulObject is SoulBlock)
            {
                soulObject.soulRenderer.sortingOrder = 6;
            }
            soulObject.targets.Clear();
        }
        Battle.b.placedSoulObjects.Clear();
        PlaceBlocks();
    }

    public static void ScaleBlocks()
    {
        foreach (SoulObject s in Battle.b.soulObjects)
        {
            s.transform.localScale = new Vector3(gridFitter.scale, gridFitter.scale, 1);
            s.relX *= gridFitter.scale;
            s.relY *= gridFitter.scale;
        }
    }

    public static void PlaceBlocks()
    {
        float minX = -1 * Camera.main.orthographicSize * Screen.width / Screen.height;
        float minY = -1 * Camera.main.orthographicSize;
        float x = minX + gridFitter.leftOffset;
        foreach (SoulObject soulObject in Battle.b.soulObjects)
        {
            if (!soulObject.placed)
            {
                float y = minY + gridFitter.bottomOffset;
                SpriteRenderer spriteRenderer = soulObject.GetComponent<SpriteRenderer>();
                x += (spriteRenderer.bounds.size.x / 2);
                y += (spriteRenderer.bounds.size.y / 2);
                soulObject.transform.position = new Vector3(x, y, 0);
                x += (spriteRenderer.bounds.size.x / 2) + gridFitter.inBetweenSpace;
            }
        }
    }
}
