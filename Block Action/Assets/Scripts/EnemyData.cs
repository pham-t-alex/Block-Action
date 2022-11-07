using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData", order = 3)]
public class EnemyData : ScriptableObject
{
    public int defaultMaxHealth;
    public int defaultStartingHealth;

    public List<string> actions;

    public Sprite idle;
    public Sprite attack;
    public Sprite hurt;
}
