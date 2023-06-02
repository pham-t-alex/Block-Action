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
        playerSprite.sortingOrder = -180;
        y += playerSprite.bounds.size.y / 2;
        Player.player.transform.position = new Vector3(-7, y, 0);
        Player.player.makeHealthBar();
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
            totalWidth += Battle.b.enemies[i].width;
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
            Enemy e = Battle.b.enemies[i];
            if (e.large)
            {
                float largeX = (Camera.main.orthographicSize * Screen.width / Screen.height) - (e.width / 2f);
                float largeY = Camera.main.orthographicSize - (e.height / 2f);
                Battle.b.enemies[i].transform.position = new Vector3(largeX, largeY, 0);
                Battle.b.enemies[i].makeHealthBar();
                if (e.type == "Big_Tree")
                {
                    Instantiate(Resources.Load<GameObject>("treehand"), Player.player.transform.position + new Vector3(-1f, -1.2f), Quaternion.identity);
                }
                continue;
            }
            x -= e.width / 2;
            y += e.height / 2;
            Battle.b.enemies[i].transform.position = new Vector3(x, y, 0);
            Battle.b.enemies[i].makeHealthBar();
            x -= e.width / 2;
            y -= e.height / 2;
            x -= fighterController.spaceBetweenEnemies;
        }
    }

    static void GenerateEnemies()
    {
        int enemyCount = 0;
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
            GameObject enemy;
            /*if (enemyInfo[0] == "Hog")
            {
                enemy = Instantiate(Resources.Load<GameObject>("Sprites/Enemies/Hog/Hog"), new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(fighterController.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            }*/
            enemy = Instantiate(fighterController.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            RuntimeAnimatorController animator = Resources.Load($"Sprites/Enemies/{enemyInfo[0]}/Animator") as RuntimeAnimatorController;
            if (animator != null)
            {
                enemy.GetComponent<Animator>().runtimeAnimatorController = animator;
            }
            else
            {
                Destroy(enemy.GetComponent<Animator>());
            }

            Battle.b.enemies.Add(enemy.GetComponent<Enemy>());
            Battle.b.fighters.Add(enemy.GetComponent<Enemy>());
            line = s.ReadLine();
        }
        s = new StringReader(waveData);
        line = s.ReadLine();
        int i = 0;
        while (line != null)
        {
            enemyCount++;
            string[] enemyInfo = line.Split(' ');
            Enemy enemy = Battle.b.enemies[i];
            enemy.setUnique("Enemy" + enemyCount);
            enemy.actionSets = new Dictionary<string, List<Action>>();
            enemy.buff = 1.0;
            enemy.taunting = false;
            enemy.defenseBuff = 1.0;
            enemy.statusEffects = new List<Status>();
            int lower = System.Convert.ToInt32(enemyInfo[4]);
            int upper = System.Convert.ToInt32(enemyInfo[5]);
            setEnemyData(enemy, enemyInfo[0], lower, upper);
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
        foreach (List<Action> actionSet in enemy.actionSets.Values)
        {
            foreach (Action action in actionSet)
            {
                foreach (Effect effect in action.effects)
                {
                    scaleEffect(effect, atkScale, buffScale);
                }
            }
        }
        foreach (Status status in enemy.statusEffects)
        {
            if (status is AtkBuffStatus)
            {
                AtkBuffStatus atkBuff = (AtkBuffStatus)status;
                enemy.buff -= atkBuff.buff;
                atkBuff.buff *= buffScale;
                enemy.buff += atkBuff.buff;
            }
            else if (status is DefBuffStatus)
            {
                DefBuffStatus defBuff = (DefBuffStatus)status;
                enemy.defenseBuff += defBuff.buff;
                defBuff.buff *= buffScale;
                enemy.defenseBuff -= defBuff.buff;
            }
            else if (status is DelayedEffectStatus)
            {
                scaleEffect(((DelayedEffectStatus)status).delayedEffect, atkScale, buffScale);
            }
            else if (status is RepeatingEffectStatus)
            {
                scaleEffect(((RepeatingEffectStatus)status).repeatingEffect, atkScale, buffScale);
            }
            else if (status is AfterActionStatus)
            {
                scaleEffect(((AfterActionStatus)status).afterActionEffect, atkScale, buffScale);
            }
            else if (status is AfterDamageStatus)
            {
                scaleEffect(((AfterDamageStatus)status).afterDamageEffect, atkScale, buffScale);
            }
            else if (status is WhenHitStatus)
            {
                scaleEffect(((WhenHitStatus)status).whenHitEffect, atkScale, buffScale);
            }
        }
        enemy.atkScale = atkScale;
        enemy.buffScale = buffScale;
    }

    static void scaleEffect(Effect effect, double atkScale, double buffScale)
    {
        Effect innerEffect = effect;
        while (innerEffect is DelayedEffect || innerEffect is RepeatingEffect || innerEffect is AfterActionEffect || innerEffect is AfterDamageEffect ||
        innerEffect is WhenHitEffect || innerEffect is ConditionalEffect || innerEffect is ScalingActionEffect)
        {
            if (innerEffect is DelayedEffect)
            {
                innerEffect = ((DelayedEffect)innerEffect).effect;
            }
            else if (innerEffect is RepeatingEffect)
            {
                innerEffect = ((RepeatingEffect)innerEffect).effect;
            }
            else if (innerEffect is AfterActionEffect)
            {
                innerEffect = ((AfterActionEffect)innerEffect).effect;
            }
            else if (innerEffect is AfterDamageEffect)
            {
                innerEffect = ((AfterDamageEffect)innerEffect).effect;
            }
            else if (innerEffect is WhenHitEffect)
            {
                innerEffect = ((WhenHitEffect)innerEffect).effect;
            }
            else if (innerEffect is ConditionalEffect)
            {
                innerEffect = ((ConditionalEffect)innerEffect).effect;
            }
            else
            {
                innerEffect = ((ScalingActionEffect)innerEffect).effect;
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

    static void setEnemyData(Enemy enemy, string enemyName, int lower, int upper)
    {
        EnemyData enemyData = Resources.Load<EnemyData>($"EnemyData/{enemyName}");
        enemy.GetComponent<SpriteRenderer>().sprite = enemyData.idle;
        enemy.width = enemyData.idle.bounds.size.x;
        enemy.height = enemyData.idle.bounds.size.y;
        enemy.type = enemyData.enemyName;
        enemy.maxHealth = enemyData.defaultMaxHealth;
        enemy.health = enemyData.defaultStartingHealth;
        enemy.actionCount = enemyData.actionsPerTurn;
        enemy.stunChargeMax = enemyData.maxStunCharge;
        enemy.baseElement = enemyData.baseElement;
        enemy.large = enemyData.large;
        if (enemy.large)
        {
            enemy.GetComponent<SpriteRenderer>().sortingOrder = -200;
        }
        enemy.boss = enemyData.boss;
        if (enemy.baseElement == Element.Elements.ELEMENTLESS)
        {
            enemy.currentElementStack = 0;
        }
        else
        {
            enemy.currentElementStack = Element.MAX_ELEMENT_STACK / 2;
        }
        for (int i = lower; i <= upper; i++)
        {
            addAction(enemy, enemyData.actions[i]);
        }
        for (int i = 0; i < enemyData.startingStatuses.Count; i++)
        {
            enemy.statusEffects.Add(Status.statusFromString(enemyData.startingStatuses[i], enemy));
        }
        enemy.minAction = lower;
        enemy.maxAction = upper;
        enemy.state = "Normal";
        enemy.currentElement = enemy.baseElement;
        enemy.soulColor = enemyData.soulColor;
    }

    static void addAction(Enemy enemy, string actionAsString)
    {
        Action a = new Action();
        a.effects = new List<Effect>();
        string[] actionData = actionAsString.Split("\n");
        string state = actionData[0];
        string[] newActionData = new string[actionData.Length - 1];
        System.Array.Copy(actionData, 1, newActionData, 0, actionData.Length - 1);
        foreach (string effectAsString in newActionData)
        {
            Effect e = Effect.effectFromString(effectAsString);
            if (e is Damage)
            {
                ((Damage)e).element = enemy.baseElement;
            }
            else if (e is DefIgnoringDamage)
            {
                ((DefIgnoringDamage)e).element = enemy.baseElement;
            }
            a.effects.Add(e);
        }
        if (!enemy.actionSets.ContainsKey(state))
        {
            enemy.actionSets.Add(state, new List<Action>());
        }
        enemy.actionSets[state].Add(a);
    }

    public static void spawnEnemy(string enemyString, Fighter summoner)
    {
        int aliveCount = 0;
        foreach (Enemy en in Battle.b.enemies)
        {
            if (!en.dead)
            {
                aliveCount++;
            }
        }
        if (aliveCount >= 3)
        {
            return;
        }

        string[] enemyInfo = enemyString.Split(",");
        GameObject e;
        /*if (enemyInfo[0] == "Hog")
        {
            e = Instantiate(Resources.Load<GameObject>("Sprites/Enemies/Hog/Hog"), new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            e = Instantiate(fighterController.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }*/
        e = Instantiate(fighterController.enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        RuntimeAnimatorController animator = Resources.Load($"Sprites/Enemies/{enemyInfo[0]}/Animator") as RuntimeAnimatorController;
        if (animator != null)
        {
            e.GetComponent<Animator>().runtimeAnimatorController = animator;
        }
        else
        {
            Destroy(e.GetComponent<Animator>());
        }

        Battle.b.enemies.Add(e.GetComponent<Enemy>());
        Battle.b.fighters.Add(e.GetComponent<Enemy>());

        Enemy enemy = e.GetComponent<Enemy>();
        enemy.setUnique("Enemy" + (Battle.b.enemies.Count + 1));
        enemy.actionSets = new Dictionary<string, List<Action>>();
        enemy.buff = 1.0;
        enemy.taunting = false;
        enemy.defenseBuff = 1.0;
        enemy.statusEffects = new List<Status>();
        int lower = System.Convert.ToInt32(enemyInfo[4]);
        int upper = System.Convert.ToInt32(enemyInfo[5]);
        setEnemyData(enemy, enemyInfo[0], lower, upper);
        enemy.gameObject.AddComponent<BoxCollider2D>();
        double hpScale = System.Convert.ToDouble(enemyInfo[1]);
        double atkScale = System.Convert.ToDouble(enemyInfo[2]);
        double buffScale = System.Convert.ToDouble(enemyInfo[3]);
        ScaleEnemy(enemy, hpScale, atkScale, buffScale);

        float y = -1 * Camera.main.orthographicSize;
        y += fighterController.bottomOffset + (enemy.height / 2);
        float center = summoner.transform.position.x;
        float x;
        if (((Enemy) summoner).large)
        {
            x = center - (((Enemy)summoner).width / 4) - (enemy.width / 2);
        }
        else
        {
            x = center - (((Enemy)summoner).width / 2) - fighterController.spaceBetweenEnemies - (enemy.width / 2);
        }

        foreach (Enemy en in Battle.b.enemies)
        {
            if (!en.dead && en != summoner && en != enemy)
            {
                if (Mathf.Abs(x - en.transform.position.x) < (en.width / 2) + (enemy.width / 2) + fighterController.spaceBetweenEnemies)
                {
                    if (((Enemy) summoner).large)
                    {
                        x = center + (((Enemy)summoner).width / 4) + (enemy.width / 2);
                    }
                    else
                    {
                        x = center + (((Enemy)summoner).width / 2) + fighterController.spaceBetweenEnemies + (enemy.width / 2);
                    }
                }
            }
        }

        enemy.transform.position = new Vector3(x, y, 0);
        enemy.makeHealthBar();
    }
}
