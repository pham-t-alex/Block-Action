using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData", order = 2)]
public class LevelData : ScriptableObject
{
    [TextArea(4, 10)]
    public string gridAsString;
    public int gridWidth;
    public int gridHeight;

    public string bgmName;

    public string background;

    [TextArea(1, 10)]
    public List<string> enemyWaves;

    [TextArea(1, 3)]
    public List<string> midLevelEffects;

    [TextArea(1, 3)]
    public List<string> firstClearRewards;
}
