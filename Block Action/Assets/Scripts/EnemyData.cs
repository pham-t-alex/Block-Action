using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData", order = 3)]
public class EnemyData : ScriptableObject
{
    public int defaultMaxHealth;
    public int defaultStartingHealth;

    [TextArea(1, 10)]
    public string description;

    public int actionsPerTurn;
    public int maxStunCharge;

    [TextArea(1, 10)]
    public List<string> actions;

    public List<string> startingStatuses;

    public Sprite idle;
    public Sprite attack;
    public Sprite hurt;
}
