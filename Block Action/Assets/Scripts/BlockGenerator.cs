using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using System.Data;

public class BlockGenerator : MonoBehaviour
{
    public GameObject block;
    public GameObject frame;
    public GameObject blockSquare;
    public GameObject frameSquare;
    public GameObject blockParticles;
    public GameObject blockInfoMenu;
    public GameObject[] icons;

    private static BlockGenerator _blockGenerator;
    public static BlockGenerator blockGenerator
    {
        get
        {
            if (_blockGenerator == null)
            {
                _blockGenerator = FindObjectOfType<BlockGenerator>();
            }
            return _blockGenerator;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (string soulObjectName in PersistentDataManager.playerBlockInventory)
        {
            Battle.b.soulObjects.Add(generateSoulObject(Resources.Load<SoulObjectData>($"BlockData/{soulObjectName}")));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static SoulObject generateSoulObject(SoulObjectData soulObjectData)
    {
        float centerX = (soulObjectData.width - 1) / 2.0f;
        float centerY = (soulObjectData.height - 1) / 2.0f;
        GameObject soulObjectGameObj;
        SoulObject soulObject;
        GameObject square;
        if (soulObjectData.isSoulBlock)
        {
            soulObjectGameObj = Instantiate(blockGenerator.block);
            soulObject = soulObjectGameObj.GetComponent<SoulBlock>();
            square = blockGenerator.blockSquare;
        } else
        {
            soulObjectGameObj = Instantiate(blockGenerator.frame);
            soulObject = soulObjectGameObj.GetComponent<SoulFrame>();
            SoulFrame frame = (SoulFrame) soulObject;
            frame.filled = false;
            square = blockGenerator.frameSquare;
        }
        soulObject.placed = false;
        soulObject.width = soulObjectData.width;
        soulObject.height = soulObjectData.height;
        soulObject.element = soulObjectData.element;

        string shape = soulObjectData.shapeAsString;
        StringReader s = new StringReader(shape);
        string line = s.ReadLine();
        float y = 0;
        float relX;
        float relY;

        PolygonCollider2D soulCollider = soulObject.GetComponent<PolygonCollider2D>();
        soulCollider.pathCount = 0;
        int sqCount = 0;

        bool relPosSet = false;
        while (line != null)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '~')
                {
                    sqCount++;
                    soulCollider.pathCount++;
                    GameObject sq = Instantiate(square);
                    sq.transform.parent = soulObject.transform;
                    relX = i - centerX;
                    relY = -1 * (y - centerY);
                    sq.transform.localPosition = new Vector3(relX, relY);
                    sq.GetComponent<SpriteRenderer>().color = soulObjectData.color;
                    if (!relPosSet)
                    {
                        soulObject.relX = relX;
                        soulObject.relY = relY;
                        relPosSet = true;
                    }
                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(relX - 0.45f, relY + 0.45f);
                    points[1] = new Vector2(relX + 0.45f, relY + 0.45f);
                    points[2] = new Vector2(relX + 0.45f, relY - 0.45f);
                    points[3] = new Vector2(relX - 0.45f, relY - 0.45f);
                    soulCollider.SetPath(soulCollider.pathCount - 1, points);

                    GameObject particles = Instantiate(blockGenerator.blockParticles);
                    ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
                    particles.transform.parent = sq.transform;
                    particles.transform.localPosition = new Vector3(0, 0.5f);
                    ParticleSystem.MainModule m = particleSystem.main;
                    m.startColor = soulObjectData.color;
                }
            }
            y++;
            line = s.ReadLine();
        }

        soulObject.originalColor = soulObjectData.color;
        soulObject.soulCollider = soulObject.GetComponent<Collider2D>();
        if (soulObject is SoulBlock)
        {
            soulObject.SetRenderOrder(6);
        }
        else if (soulObject is SoulFrame)
        {
            soulObject.SetRenderOrder(3);
        }
        soulObject.squareCount = sqCount;
        soulObject.defaultCooldown = soulObjectData.cooldown;

        foreach (string effectAsString in soulObjectData.effects)
        {
            Effect e = Effect.effectFromString(effectAsString);
            if (e is Damage)
            {
                ((Damage)e).element = soulObject.element;
            }
            else if (e is DefIgnoringDamage)
            {
                ((DefIgnoringDamage)e).element = soulObject.element;
            }
            soulObject.effects.Add(e);
        }
        soulObject.soulName = soulObjectData.soulName;
        soulObject.description = soulObjectData.description;
        soulObject.element = soulObjectData.element;
        soulObject.altBlock = soulObjectData.altBlock;

        List<GameObject> effectIcons = new List<GameObject>();

        /**
        foreach (string effect in soulObjectData.effects) {
            Debug.Log(soulObject + " DATA: " + effect);
        }
        foreach (Effect effect in soulObject.effects)
        {
            Debug.Log(soulObject + "EFFECT: " + effect);
        }
        */

        List<GameObject> iconList = new List<GameObject>();
        
        // Add icons to the iconList
        foreach (GameObject icon in blockGenerator.icons) {
            iconList.Add(icon);
        }

        // Scans each description and adds appropriate effect
        /*foreach (string data in soulObjectData.effects)
        {
            List<GameObject> selectableIcons = iconList;
            if (data.Contains("damage enemies")) {
                if (selectableIcons[0] != null) {
                    effectIcons.Add(selectableIcons[0]); // AoE Attack icon
                    selectableIcons[0] = null;
                }
            }
            if (data.Contains("buff") && data.Contains("atk")) {
                if (selectableIcons[1] != null) {
                    effectIcons.Add(selectableIcons[1]); // Atk Buff icon
                    selectableIcons[1] = null;
                }
            }
            if (data.Contains("remove_debuff")) {
                if (selectableIcons[3] != null) {
                    effectIcons.Add(selectableIcons[3]); // Debuff Removal icon
                    selectableIcons[3] = null;
                }
            }
            if (data.Contains("buff") && data.Contains("def")) {
                if (selectableIcons[4] != null) {
                    effectIcons.Add(selectableIcons[4]); // Defense Buff icon
                    selectableIcons[4] = null;
                }
            }
            if (data.Contains("delayed") || data.Contains("apply_after_damge")) {
                if (selectableIcons[6] != null) {
                    effectIcons.Add(selectableIcons[6]); // Delayed icon
                    selectableIcons[6] = null;
                }
            }
            if (data.Contains("heal")) {
                if (selectableIcons[7] != null) {
                    effectIcons.Add(selectableIcons[7]); // Heal icon
                    selectableIcons[7] = null;
                }
            }
            if (data.Contains("singletarget")) {
                if (selectableIcons[9] != null) {
                    effectIcons.Add(selectableIcons[9]); // Single Target icon
                    selectableIcons[9] = null;
                }
            }
        }*/
        List<GameObject> selectableIcons = iconList;
        foreach (Effect effect in soulObject.effects)
        {
            Effect e = effect;
            while (e is ConditionalEffect)
            {
                e = ((ConditionalEffect)e).effect;
            }
            if (e is Damage || e is DefIgnoringDamage || e is TrueDamage)
            {
                if (e.targetType == TargetType.AllEnemies)
                {
                    effectIcons.Add(selectableIcons[0]);
                }
                else
                {
                    effectIcons.Add(selectableIcons[9]);
                }
            }
            else if (e is Buff)
            {
                Buff b = (Buff)e;
                if (b.buff >= 0)
                {
                    effectIcons.Add(selectableIcons[1]);
                }
                else
                {
                    effectIcons.Add(selectableIcons[2]);
                }
            }
            else if (e is DebuffRemovalEffect)
            {
                effectIcons.Add(selectableIcons[3]);
            }
            else if (e is DefenseBuff)
            {
                DefenseBuff b = (DefenseBuff)e;
                if (b.defenseBuff >= 0)
                {
                    effectIcons.Add(selectableIcons[4]);
                }
                else
                {
                    effectIcons.Add(selectableIcons[5]);
                }
            }
            else if (e is DelayedEffect || e is AfterActionEffect || e is AfterDamageEffect || e is WhenHitEffect)
            {
                effectIcons.Add(selectableIcons[6]);
            }
            else if (e is Heal)
            {
                effectIcons.Add(selectableIcons[7]);
            }
            else if (e is RepeatingEffect)
            {
                effectIcons.Add(selectableIcons[8]);
            }
        }

        int effectCount = effectIcons.Count;
        float iconSpacing = 0.8f;
        float iconXPosition = (float)((iconSpacing / 2) - (effectCount * iconSpacing / 2));

        // Calculates and creates icons
        foreach (GameObject icon in effectIcons) {
            // Debug.Log(soulObject.soulName + "   " + effectCount + "   " + icon.name + "   " + soulObject.height);
            var soulObjectIcon = Instantiate(icon, soulObject.transform); // Create icon
            soulObjectIcon.transform.position = soulObject.transform.position; // Set icon as parent of soulObject
            soulObjectIcon.transform.position += new Vector3(iconXPosition, -0.4f - (0.5f * soulObject.height), 0); // Adds icon at iconXPosiiton
            iconXPosition += iconSpacing; // Increments iconXPosition
        }
        
        return soulObject;
    }
}
