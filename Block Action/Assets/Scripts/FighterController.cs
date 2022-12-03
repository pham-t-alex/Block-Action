using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FighterController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject fighterInfoMenu;
    public float spaceBetweenEnemies;
    public float bottomOffset;
    public float minRightOffset;

    public LevelData levelData;
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
        levelData = Resources.Load<LevelData>($"Levels/Level {levelNumber}");
        AudioController.audioController.PlayBGM(levelData.bgm, levelData.bgmRepeat);
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
            Battle.b.enemies.Add(enemy.GetComponent<Enemy>());
            Battle.b.fighters.Add(enemy.GetComponent<Enemy>());
            line = s.ReadLine();
        }
        s = new StringReader(waveData);
        line = s.ReadLine();
        int i = 0;
        while (line != null)
        {
            string[] enemyInfo = line.Split(' ');
            Enemy enemy = Battle.b.enemies[i];
            enemy.effects = new List<Effect>();
            enemy.buff = 1.0;
            enemy.defenseBuff = 1.0;
            enemy.buffLeft = new List<BuffCounter>();
            enemy.defenseBuffLeft = new List<DefenseBuffCounter>();
            int lower = System.Convert.ToInt32(enemyInfo[4]);
            int upper = System.Convert.ToInt32(enemyInfo[5]);
            enemy.type = enemyInfo[0];
            setEnemyData(enemy, enemyInfo[0], lower, upper);
            enemy.numAtk = enemy.effects.Count;
            enemy.gameObject.AddComponent<BoxCollider2D>();
            double hpScale = System.Convert.ToDouble(enemyInfo[1]);
            double atkScale = System.Convert.ToDouble(enemyInfo[2]);
            double buffScale = System.Convert.ToDouble(enemyInfo[3]);
            ScaleEnemy(enemy, hpScale, atkScale, buffScale);
            line = s.ReadLine();
            i++;
        }
    }

    static void ScaleEnemy(Enemy enemy, double hpScale, double atkScale, double buffScale)
    {
        enemy.health = (int) (enemy.health * hpScale);
        enemy.maxHealth = (int) (enemy.maxHealth * hpScale);
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
        enemy.atkScale = atkScale;
        enemy.buffScale = buffScale;
    }

    static void setEnemyData(Enemy enemy, string enemyName, int lower, int upper)
    {
        EnemyData enemyData = Resources.Load<EnemyData>($"EnemyData/{enemyName}");
        enemy.GetComponent<SpriteRenderer>().sprite = enemyData.idle;
        enemy.maxHealth = enemyData.defaultMaxHealth;
        enemy.health = enemyData.defaultStartingHealth;
        for (int i = lower; i <= upper; i++)
        {
            addEffect(enemy, enemyData.actions[i]);
        }
        enemy.minAction = lower;
        enemy.maxAction = upper;
    }

    static void addEffect(Enemy enemy, string effectAsString)
    {
        string[] effectData = effectAsString.Split(" ");
        Effect effect = null;
        if (effectData[0].Equals("dmg"))
        {
            effect = new Damage(System.Convert.ToInt32(effectData[2]));
        }
        else if (effectData[0].Equals("heal"))
        {
            effect = new Heal(System.Convert.ToDouble(effectData[2]));
        }
        else if (effectData[0].Equals("buff"))
        {
            if (effectData[2].Equals("atk"))
            {
                effect = new Buff(System.Convert.ToDouble(effectData[3]));
                effect.numTurns = System.Convert.ToInt32(effectData[4]);
            }
            else if (effectData[2].Equals("def"))
            {
                effect = new DefenseBuff(System.Convert.ToDouble(effectData[3]));
                effect.numTurns = System.Convert.ToInt32(effectData[4]);
            }
        }
        if (effectData[1].Equals("player"))
        {
            effect.targets.Add(Player.player);
        }
        else if (effectData[1].Equals("self"))
        {
            effect.self = true;
        }
        else if (effectData[1].Equals("enemies"))
        {
            foreach (Enemy e in Battle.b.enemies)
            {
                effect.targets.Add(e);
            }
        }
        enemy.effects.Add(effect);
    }
}
