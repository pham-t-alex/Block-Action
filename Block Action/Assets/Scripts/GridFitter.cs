using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFitter : MonoBehaviour
{
    private static GridFitter _gridFitter;
    // COMEBACK
    public static float defaultScale = 0.5f;
    public static Vector3 defaultSize = new Vector3(defaultScale, defaultScale, 1);

    public static GridFitter gridFitter {
        get
        {
            if (_gridFitter == null)
            {
                _gridFitter = FindObjectOfType<GridFitter>();
            }
            return _gridFitter;
        }
    } //Singleton object version, accessible through GridFitter.gridFitter, and will never throw NullPointerException

    public GameObject target; //target sprite gameobject
    static SoulObject selectedSoulObject; //currently selected soul object
    static float selectedTime = float.MinValue; //used to allow object to follow mouse around for a short amount of time after being selected
    //This was added since it makes the dragging of the block more smooth. Without this feature, if the mouse exits the bounds of the block, it would
    //become unselected, so if you drag too fast, it would stop. This buffer would allow dragging to be more smooth, while still allowing the player
    //to stop selecting a block.
    static Vector3 prevMousePosition = new Vector3(0, 0); //the previous mouse position, to check delta mouse position
    static Vector3 prevObjectPosition = new Vector3(0, 0); //the previous object position
    public Grid grid; //the grid object

    //For block placement
    public float leftOffset; //offset between the left block and the left edge, in units
    public float inBetweenSpace; //space between the blocks in units
    public float bottomOffset; //space between the blocks and the bottom, in units
    public float rightOffset; //space between blocks and the grid in units
    public float scale; //default 1 (1x), can be modified to make blocks and grid bigger or smaller


    void Start()
    {
        target.GetComponent<SpriteRenderer>().sortingOrder = 100; //sets target in front
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Method ran in Battle class, ran so long as the battle state is grid fitting phase
    public static void GridFitting()
    {
        gridFitter.target.SetActive(false); //Makes the target gameobject inactive
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse position
        mousePosition.z = 0; //Sets mouse position z to 0, since 2D

        if (selectedSoulObject != null) //If an object is selected
        {
            if (selectedTime > 0) //While the object is selected, or a little bit after (see selectedTime variable for more info)
            {
                selectedSoulObject.transform.position += mousePosition - prevMousePosition; //Changes the soul object's position by the change in mouse position
                //This is better than simply setting the soul object's position to the mouse position, since that would snap the soul object's center to the
                //mouse position. With this, you can drag the soul object from its edge, it feels more natural.
                // COMEBACK
                selectedSoulObject.transform.localScale = new Vector3(gridFitter.scale, gridFitter.scale, 1);
            }
            else if (selectedTime > float.MinValue) //When the object stops being selected (<0)
            {
                selectedTime = float.MinValue; //Sets selected time to min float value, which will prevent this from looping
                placeSoulObject(selectedSoulObject); //Attempts to place the soul object (see place soul object method)
            }
        }
        if (Input.GetMouseButton(0)) //If mouse down
        {
            if (selectedSoulObject == null)
            {
                foreach (SoulObject soulObject in Battle.b.soulObjects) //Look at all existing soul objects
                {
                    if (soulObject.mouseTouching)
                    {
                        if (soulObject.placed)
                        {
                            gridFitter.grid.soulObjectsInGrid.Remove(soulObject);
                            foreach (GameObject t in soulObject.tilesTouching)
                            {
                                if (soulObject is SoulBlock)
                                {
                                    t.GetComponent<Tile>().filled = false;
                                    Battle.b.placedSoulObjects.Remove(soulObject);
                                }
                                if (soulObject is SoulFrame)
                                {
                                    t.GetComponent<Tile>().framed = false;
                                    if (Battle.b.placedSoulObjects.Contains(soulObject))
                                    {
                                        Battle.b.placedSoulObjects.Remove(soulObject);
                                    }
                                    SoulFrame s = (SoulFrame) soulObject;
                                    s.filled = false;
                                }
                                soulObject.targets.Clear();
                                updateFrames();
                            }
                            soulObject.tilesTouching = null;
                            soulObject.placed = false;
                        }
                        selectedSoulObject = soulObject; //selects the soul object
                        if (soulObject is SoulBlock)
                        {
                            soulObject.SetRenderOrder(10);
                            soulObject.transform.localScale = defaultSize;
                        }
                        else
                        {
                            soulObject.SetRenderOrder(4);
                        } //sets display order in layer for display
                        selectedTime = 0.05f; //sets selected time to 0.05 seconds (after 0.05 seconds of not being touched by mouse down, it will deselect)
                    }
                }
            }
        }
        else
        {
            selectedTime -= Time.deltaTime; //decrements selected time
        }
        prevMousePosition = mousePosition; //sets previous mouse position to current mouse position
    }

    //Method that attempts to place a soul object in the grid
    //If it works, it is placed, and if not, then it unselects the object
    static void placeSoulObject(SoulObject soulObject)
    {
        List<GameObject> touchingTiles = new List<GameObject>(); // creation of array list
        foreach (GameObject t in gridFitter.grid.tiles) // test if block is touching all tiles
        {
            if (soulObject is SoulBlock) //if it's a block
            {
                if (!t.GetComponent<Tile>().filled && t.GetComponent<Tile>().TouchingSoulObject(soulObject)) //makes sure the tiles is not filled already, and is touching the block's collider
                {
                    touchingTiles.Add(t); //adds it to the list of touching tiles
                }
            }
            else //if it's a frame
            {
                if (!t.GetComponent<Tile>().filled && !t.GetComponent<Tile>().framed && t.GetComponent<Tile>().TouchingSoulObject(soulObject)) //makes sure tiles are not filled and not framed
                {
                    touchingTiles.Add(t);
                }
            }
            //the difference is that frames can't be placed on framed tiles, while blocks can
        }

        if (touchingTiles.Count >= soulObject.squareCount) //if the soul object's collider touches a sufficient quantity of tiles; if the placement is legal
        {
            soulObject.tilesTouching = touchingTiles;
            if (soulObject is SoulBlock) //if it's a soul block
            {
                foreach (GameObject tile in touchingTiles)
                {
                    tile.GetComponent<Tile>().filled = true; //makes the tiles filled
                }
            }
            else
            {
                foreach (GameObject tile in touchingTiles)
                {
                    tile.GetComponent<Tile>().framed = true; //makes the tiles framed
                }
            }
            float refX = soulObject.transform.position.x + soulObject.relX; //sets a reference x to the middle of one of the 1x1 blocks of the soulObject
            float refY = soulObject.transform.position.y + soulObject.relY; //sets a reference y to the middle of one of the 1x1 blocks of the soulObject
            //These two, along with relX and relY, are used to snap the block to the nearest tile position. By creating a reference position that is the
            //center of one of the 1x1 blocks that make up the soul object, it can find the closest tile to the reference position and shift the soulObject
            //so that its reference position aligns with the closest tile's center. This would make the 1x1 block perfectly align with the tile, and therefore
            //make the rest of the soulObject perfectly align with the grid.

            GameObject t = closestTile(touchingTiles, refX, refY); //finds closest tile to the reference position

            soulObject.transform.position += new Vector3(t.transform.position.x - (refX), t.transform.position.y - (refY), 0); //snaps soulObject to grid

            soulObject.placed = true; //sets placed to true
            if (soulObject is SoulBlock)
            {
                soulObject.SetRenderOrder(5);
                Battle.b.placedSoulObjects.Add(soulObject); //added to placed soul objects
                updateFrames(); //if it is a block, it has to check to see if it filled any frames
            }
            else
            {
                soulObject.SetRenderOrder(2);
                //since placed soul objects is used to mark soul objects that will be activated, frames aren't added yet, since they are not activated
                //until a block goes on them
            }

            gridFitter.grid.soulObjectsInGrid.Add(soulObject); //added to soul objects in grid
            if (soulObject.isAoe) //if the soul object is AOE
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    soulObject.targets.Add(e); //add every target
                }
                selectedSoulObject = null; //unselect the object
            }
            else if (soulObject.isSingleTarget)
            {
                Battle.b.bs = BattleState.EnemySelect; //if it is single target, go to enemy select phase
            }
            else
            {
                selectedSoulObject = null; //unselect the object
            }
        }
        else //failed placement
        {
            selectedSoulObject.transform.position = selectedSoulObject.startPosition; //return the object to previous position
            // COMEBACK
            soulObject.transform.localScale = defaultSize;
            selectedSoulObject = null; //unselect
            if (soulObject is SoulBlock)
            {
                soulObject.SetRenderOrder(6);
            }
            else
            {
                soulObject.SetRenderOrder(3);
            }
        }
    }

    //Find closest tile to a position
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

    //Find the distance between the center of a tile and a position
    //Used in finding closest tile
    static float distanceBetween(GameObject t, float refX, float refY)
    {
        return Vector3.Distance(t.transform.position - new Vector3(0, 0, t.transform.position.z), new Vector3(refX, refY, 0));
    }

    //Update frames upon block placement
    static void updateFrames()
    {
        foreach (SoulObject soulObject in gridFitter.grid.soulObjectsInGrid)
        {
            if (soulObject is SoulFrame) //for every soul frame
            {
                SoulFrame soulFrame = (SoulFrame)soulObject;
                int count = 0;
                foreach (GameObject t in gridFitter.grid.tiles) //check if tiles that the soul frame is touching are filled
                {
                    if (t.GetComponent<Tile>().filled && t.GetComponent<Tile>().TouchingSoulObject(soulFrame))
                    {
                        count++;
                    }
                }
                if (count >= soulFrame.squareCount) //if enough tiles are filled, then add the soul frame to activated objects
                {
                    if (!soulFrame.filled)
                    {
                        soulFrame.filled = true;
                        soulObject.SetRenderOrder(20);
                        Battle.b.placedSoulObjects.Add(soulObject);
                    }
                }
                else
                {
                    soulFrame.filled = false;
                    soulObject.SetRenderOrder(2);
                    if (Battle.b.placedSoulObjects.Contains(soulObject))
                    {
                        Battle.b.placedSoulObjects.Remove(soulObject);
                    }
                }
            }
        }
    }

    //Enemy select phase
    public static void EnemySelect()
    {
        gridFitter.target.SetActive(true); //show the target
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
                    //if mouse is touching an enemy and the player clicks, then add that enemy to the selected soul object's target list
                    //unselect the soul object, return to grid fitting phase
                }
                else
                {
                    gridFitter.target.transform.position = enemy.transform.position;
                    //if mouse is not pressed, but still touching the enemy, snap the target to the enemy position. This indicates to the
                    //player that clicking would target that enemy.
                }
                break;
            }
        }
        if (!touchingEnemy) //if not touching the enemy
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            gridFitter.target.transform.position = mousePosition; //teleport target to mouse
        }
    }

    //reset soul objects, happens after enemy phase
    public static void ResetSoulObjects()
    {
        foreach (SoulObject soulObject in Battle.b.soulObjects)
        {
            foreach (GameObject t in gridFitter.grid.tiles)
            {
                if (t.GetComponent<Tile>().TouchingSoulObject(soulObject))
                {
                    if (soulObject is SoulBlock)
                    {
                        t.GetComponent<Tile>().filled = false; //unfills the tiles under the soul block
                    }
                    else if (soulObject is SoulFrame)
                    {
                        t.GetComponent<Tile>().framed = false; //unframes the tiles under the soul frame
                    }
                }
            }
            soulObject.placed = false; //makes the object no longer placed
            if (soulObject is SoulFrame)
            {
                soulObject.SetRenderOrder(3);
                SoulFrame s = (SoulFrame)soulObject;
                s.filled = false; //unfills soul frame
                
            }
            else if (soulObject is SoulBlock)
            {
                soulObject.SetRenderOrder(6);
            }
            soulObject.targets.Clear(); //gets rid of targets
            soulObject.tilesTouching = null;
        }
        Battle.b.placedSoulObjects.Clear(); //clears activated soul objects
        GridFitter.gridFitter.grid.soulObjectsInGrid.Clear();
        PlaceBlocks(); //places blocks at their starting position
    }

    //scales blocks, done at beginning of battle
    public static void ScaleBlocks()
    {
        foreach (SoulObject s in Battle.b.soulObjects)
        {
            s.transform.localScale = new Vector3(gridFitter.scale, gridFitter.scale, 1);
            s.relX *= gridFitter.scale;
            s.relY *= gridFitter.scale;
        }
    }

    //places blocks (at beginning of battle, and on reset)
    public static void PlaceBlocks()
    {
        float minX = -1 * Camera.main.orthographicSize * Screen.width / Screen.height; //get left edge x coordinate
        float minY = -1 * Camera.main.orthographicSize; //get bottom edge y coordinate
        float maxX = 0 - (Grid.scale * FighterController.fighterController.levelData.gridWidth / 2);
        float x = minX + gridFitter.leftOffset; //sets x to left edge + left offset
        float maxHeight = 0;
        foreach (SoulObject soulObject in Battle.b.soulObjects)
        {
            // COMEBACK
            soulObject.transform.localScale = defaultSize;
            if (!soulObject.placed) //if the soul object is not placed (this is important to not teleport frames back, they stay between turns)
            {
                float blockRightEdge = x + (defaultScale * soulObject.width);
                if (maxX - blockRightEdge <= gridFitter.rightOffset)
                {
                    x = minX + gridFitter.leftOffset; //sets x to left edge + left offset
                    minY += gridFitter.inBetweenSpace + maxHeight;
                    maxHeight = 0;
                }
                float y = minY + gridFitter.bottomOffset; //sets y to bottom edge + bottom offset
                x += (defaultScale * soulObject.width / 2); //increments position by half of the object's size (because the object's position is at its center)
                y += (defaultScale * soulObject.height / 2); //increments position by half of the object's size
                soulObject.transform.position = new Vector3(x, y, 0); //sets position to x, y
                soulObject.startPosition = soulObject.transform.position;
                x += (defaultScale * soulObject.width / 2) + gridFitter.inBetweenSpace; //increments x by half of object's size (to reach the right edge of the object) and inbetween space
                maxHeight = System.Math.Max(maxHeight, defaultScale * soulObject.height);
            }
            if (soulObject.currentCooldown > 0)
            {
                soulObject.currentCooldown--;
            }
            soulObject.changeCooldownColor();
        }
    }
}
