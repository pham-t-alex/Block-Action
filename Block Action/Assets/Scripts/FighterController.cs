using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FighterController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spaceBetweenEnemies;
    public float bottomOffset;
    public float minRightOffset;
    EnemySprites enemySprites;

    LevelData levelData;
    public int levelNumber;
    public int wave;

    private static FighterController _fighterController;
    public static FighterController fighterController
    {
        get
        {
            if (_fighterController == null)
            {
                _fighterController = FindObjectOfType<FighterController>();
            }
            return _fighterController;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySprites = Resources.Load<EnemySprites>("EnemySprites");
        levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaceFighters()
    {
        PlacePlayer();
        PlaceEnemies();
    }

    static void PlacePlayer()
    {
        float y = -1 * Camera.main.orthographicSize;
        y += fighterController.bottomOffset;
        SpriteRenderer playerSprite = Player.player.GetComponent<SpriteRenderer>();
        y += playerSprite.bounds.size.y / 2;
        Player.player.transform.position = new Vector3(-7, y, 0);
    }

    static void PlaceEnemies()
    {
        GenerateEnemies();
        float y = -1 * Camera.main.orthographicSize;
        y += fighterController.bottomOffset;
        float totalWidth = 0;
        for (int i = 0; i < Battle.b.enemies.Count; i++)
        {
            if (i > 0)
            {
                totalWidth += fighterController.spaceBetweenEnemies;
            }
            totalWidth += Battle.b.enemies[i].GetComponent<SpriteRenderer>().bounds.size.x;
        }
        float rightOffset = 0;
        if (totalWidth < (Camera.main.orthographicSize * Screen.width / Screen.height) - 2 * (fighterController.minRightOffset))
        {
            rightOffset = ((Camera.main.orthographicSize * Screen.width / Screen.height) - totalWidth) / 2;
        } else
        {
            rightOffset = fighterController.minRightOffset;
        }
        float x = (Camera.main.orthographicSize * Screen.width / Screen.height) - rightOffset;
        for (int i = Battle.b.enemies.Count - 1; i >= 0; i--)
        {
            SpriteRenderer spriteRenderer = Battle.b.enemies[i].GetComponent<SpriteRenderer>();
            x -= spriteRenderer.bounds.size.x / 2;
            y += spriteRenderer.bounds.size.y / 2;
            Battle.b.enemies[i].transform.position = new Vector3(x, y, 0);
            x -= spriteRenderer.bounds.size.x / 2;
            y -= spriteRenderer.bounds.size.y / 2;
            x -= fighterController.spaceBetweenEnemies;
        }
    }

    static void GenerateEnemies()
    {
        string waveData = fighterController.levelData.enemyWaves[fighterController.wave - 1];
        StringReader s = new StringReader(waveData);
        string line = s.ReadLine();
        while (line != null)
        {
            string[] enemyInfo = line.Split(' ');
            GameObject enemy = Instantiate(fighterController.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            enemy.GetComponent<SpriteRenderer>().sprite = fighterController.enemySprites.GetSprite(enemyInfo[0]);
            enemy.AddComponent<BoxCollider2D>();
            double hpScale = System.Convert.ToDouble(enemyInfo[1]);
            double atkScale = System.Convert.ToDouble(enemyInfo[2]);
            double buffScale = System.Convert.ToDouble(enemyInfo[3]);
            enemy.GetComponent<Enemy>().initialize();
            ScaleEnemy(enemy.GetComponent<Enemy>(), hpScale, atkScale, buffScale);
            Battle.b.enemies.Add(enemy.GetComponent<Enemy>());
            Battle.b.fighters.Add(enemy.GetComponent<Enemy>());
            line = s.ReadLine();
        }
    }

    static void ScaleEnemy(Enemy enemy, double hpScale, double atkScale, double buffScale)
    {
        enemy.health = (int) (enemy.health * hpScale);
        foreach (Effect effect in enemy.effects)
        {
            if (effect is Damage)
            {
                Damage damageEffect = (Damage) effect;
                damageEffect.dmg *= atkScale;
            }
            if (effect is Heal)
            {
                Heal healEffect = (Heal) effect;
                healEffect.heal *= atkScale;
            }
            if (effect is Buff)
            {
                Buff buffEffect = (Buff) effect;
                buffEffect.buff *= buffScale;
            }
        }
    }
}
