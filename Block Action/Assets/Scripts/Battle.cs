using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public List<SoulObject> soulObjects;
    //Note: potentially not necessary if using grid.soulObjectsInGrid;
    public List<SoulObject> placedSoulObjects;
    public BattleState bs;
    public static Battle b;
    public List<Enemy> enemies;

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
