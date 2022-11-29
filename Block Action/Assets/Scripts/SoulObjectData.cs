using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoulObject", menuName = "ScriptableObjects/SoulObjectData", order = 1)]
public class SoulObjectData : ScriptableObject
{
    public string soulName;
    [TextArea(1, 10)]
    public string description;
    [TextArea(4, 10)]
    public string shapeAsString;
    public int width;
    public int height;
    public string element;
    public int cooldown;
    public bool isSoulBlock;

    public List<string> effects;

    public Color color;
}