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
        if (Battle.b.levelData == null)
        {
            return;
        }
        string waveData = Battle.b.levelData.enemyWaves[Battle.b.wave - 1];
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
            enemy.actions = new List<Action>();
            enemy.buff = 1.0;
            enemy.defenseBuff = 1.0;
            enemy.statusEffects = new List<Status>();
            int lower = System.Convert.ToInt32(enemyInfo[4]);
            int upper = System.Convert.ToInt32(enemyInfo[5]);
            enemy.type = enemyInfo[0];
            setEnemyData(enemy, enemyInfo[0], lower, upper);
            enemy.numAtk = enemy.actions.Count;
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
        foreach (Action action in enemy.actions)
        {
            foreach (Effect effect in action.effects)
            {
                Effect innerEffect = effect;
                while (innerEffect is DelayedEffect || innerEffect is RepeatingEffect)
                {
                    if (innerEffect is DelayedEffect)
                    {
                        innerEffect = ((DelayedEffect)innerEffect).effect;
                    }
                    else
                    {
                        innerEffect = ((RepeatingEffect)innerEffect).effect;
                    }
                }
                if (innerEffect is Damage)
                {
                    Damage damageEffect = (Damage)innerEffect;
                    damageEffect.dmg *= atkScale;
                }
                if (innerEffect is TrueDamage)
                {
                    TrueDamage damageEffect = (TrueDamage)innerEffect;
                    damageEffect.dmg *= atkScale;
                }
                if (innerEffect is DefIgnoringDamage)
                {
                    DefIgnoringDamage damageEffect = (DefIgnoringDamage)innerEffect;
                    damageEffect.dmg *= atkScale;
                }
                if (innerEffect is Heal)
                {
                    Heal healEffect = (Heal)innerEffect;
                    healEffect.heal *= atkScale;
                }
                if (innerEffect is Buff)
                {
                    Buff buffEffect = (Buff)innerEffect;
                    buffEffect.buff *= buffScale;
                }
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
        enemy.actionCount = enemyData.actionsPerTurn;
        enemy.stunChargeMax = enemyData.maxStunCharge;
        for (int i = lower; i <= upper; i++)
        {
            addAction(enemy, enemyData.actions[i]);
        }
        enemy.minAction = lower;
        enemy.maxAction = upper;
    }

    static void addAction(Enemy enemy, string actionAsString)
    {
        Action a = new Action();
        a.effects = new List<Effect>();
        string[] actionData = actionAsString.Split("\n");
        foreach (string effectAsString in actionData)
        {
            a.effects.Add(Effect.effectFromString(effectAsString));
        }
        enemy.actions.Add(a);
    }
}
