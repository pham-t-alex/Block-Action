using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BlockGenerator : MonoBehaviour
{
    public GameObject block;
    public GameObject frame;
    public GameObject blockSquare;
    public GameObject frameSquare;
    public GameObject blockParticles;
    public GameObject blockInfoMenu;

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
            string[] effectData = effectAsString.Split(" ");
            Effect effect = null;
            if (effectData[0].Equals("damage"))
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
            if (effectData[1].Equals("self"))
            {
                effect.targetType = TargetType.Self;
            }
            else if (effectData[1].Equals("enemies"))
            {
                effect.targetType = TargetType.AllEnemies;
            }
            else if (effectData[1].Equals("enemy"))
            {
                effect.targetType = TargetType.SingleTarget;
            }
            soulObject.effects.Add(effect);
        }
        soulObject.soulName = soulObjectData.soulName;
        soulObject.description = soulObjectData.description;
        soulObject.element = soulObjectData.element;

        return soulObject;
    }
}
