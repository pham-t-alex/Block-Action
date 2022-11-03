using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    private static Battle _b;
    public static Battle b
    {
        get
        {
            if (_b == null)
            {
                _b = FindObjectOfType<Battle>();
            }
            return _b;
        }
    }
    public List<Enemy> enemies;
    public List<Fighter> fighters;

    // Start is called before the first frame update
    void Start()
    {
        //level initialization

        //for player
        bs = BattleState.PlayerGrid;
        Effect e1 = new Damage(50);
        Effect e2 = new Heal(10);
        Effect e3 = new Damage(5);
        Effect e4 = new Buff(2);
        e1.self = false;
        e2.self = true;
        e3.self = false;
        e4.self = true;
        e4.numTurns = 2;
        soulObjects[0].effects.Add(e1); 
        soulObjects[1].effects.Add(e2);
        soulObjects[1].effects.Add(e4);
        soulObjects[5].effects.Add(e3);

        Effect e5 = new Damage(40);
        e5.self = false;
        Effect e6 = new Heal(20);
        e6.self = true;
        Effect e7 = new Buff(1.2);
        e7.self = true;
        Effect e8 = new Damage(10);
        e8.self = false;

        soulObjects[2].effects.Add(e5);
        soulObjects[3].effects.Add(e6);
        soulObjects[3].effects.Add(e7);
        soulObjects[4].effects.Add(e8);


        //grid initialization
        Grid.SetScale();
        GridFitter.ScaleBlocks();
        GridFitter.PlaceBlocks();
        FighterController.PlaceFighters();
    }

    // Update is called once per frame
    void Update()
    {
        if (bs.Equals(BattleState.PlayerGrid)) {
            GridFitter.GridFitting();
        } else if (bs.Equals(BattleState.PlayerAction)) {
            ActionController.PlayerTurn();
        } else if (bs.Equals(BattleState.EnemyAction)) {
            ActionController.EnemyTurn();
        } else if (bs.Equals(BattleState.EnemySelect)) {
            GridFitter.EnemySelect();
        }
    }
}
