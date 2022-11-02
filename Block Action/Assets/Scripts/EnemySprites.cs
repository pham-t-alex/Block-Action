using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySprites", menuName = "ScriptableObjects/EnemySprites", order = 1)]
public class EnemySprites : ScriptableObject
{
    public List<string> spriteNames;
    public List<Sprite> sprites;

    public Sprite GetSprite(string name)
    {
        int index = spriteNames.IndexOf(name);
        if (index == -1)
        {
            return null;
        }
        return sprites[index];
    }
}
