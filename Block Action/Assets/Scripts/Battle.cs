using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    Vector3 prevMousePosition = new Vector3(0, 0);
    public Grid grid;
    public List<SoulBlock> soulBlocks;
    public List<SoulBlock> placedBlocks;        //this should be a list of placed blocks in grid
    public BattleState bs;
    public static Battle b;
    public List<Enemy> enemies;

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
        foreach (SoulBlock block in placedBlocks) {
            //attack animation
            PlayerSequence(block);
        }
        placedBlocks.Clear();
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

    void PlayerSequence(SoulBlock s) {
        //after single target, where do we save the attack enemy?

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
            if (Player.player.health > Player.player.MaxHealth) { //this number shouldn't be hard coded
                Player.player.health = Player.player.MaxHealth;
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
