using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentDataManager
{
    public static List<string> playerBlockInventory = new List<string>();
    public static List<int> levelsCompleted = new List<int>(new int[1]);
}
