using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SummonEffect : Effect
{
    public string[] enemies;
    public SummonEffect(string[] enemies)
    {
        this.enemies = enemies;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        Random rand = new Random();
        int i = rand.Next(0, enemies.Length);
        FighterController.spawnEnemy(enemies[i], fighter);
    }
}
