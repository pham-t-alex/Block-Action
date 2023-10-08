using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentDataManager
{
    public static List<string> playerBlockInventory = new List<string>();
    public static int levelsCompleted = -1;

    public static int levelNumber = -1; //-1 = invalid
    public static bool bonusVariant = false;
    public static int storyState = 0; //0 = invalid, 1 = pre-battle, 2 = post-battle
    public static bool storyOnly = false;

    public static bool devMode = false;
}   