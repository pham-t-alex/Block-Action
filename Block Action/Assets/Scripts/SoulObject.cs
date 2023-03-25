using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Base class representing a SoulObject placeable in the grid.
public class SoulObject : MonoBehaviour
{
    public List<GameObject> tilesTouching;
    //Whether the SoulObject is placed in the grid or not
    public bool placed;
    //Whether the mouse is touching the SoulObject
    public bool mouseTouching;
    //Number of squares that make up the SoulObject
    //= number of tiles it takes up on the grid
    public int squareCount;
    public Collider2D soulCollider;
    //Adding relX and relY to the transform position of
    //the SoulObject gives a point that lies at the center
    //of one of the squares that makes up the SoulObject
    public float relX;
    public float relY;

    public int width;
    public int height;

    public string soulName;
    public string description;

    // Variables required for cooldown calculations
    public int defaultCooldown;
    public int currentCooldown;

    // Variable for initiating cooldown text
    public GameObject cooldownIndicator = null;
    public int layer;

    // Variable used for changing soul color when on cooldown
    private SpriteRenderer _spriteRenderer;

    public List<Effect> effects = new List<Effect>();
    public Element.Elements element;

    public Color originalColor;

    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        soulCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseOver()
    {
        CreateInfoMenu();
    }

    void OnMouseEnter()
    {
        if (currentCooldown == 0 && !Player.player.stunned) // If block is on cooldown, disable mouse interaction
        {
            mouseTouching = true;
        }
    }

    void OnMouseExit()
    {
        mouseTouching = false;
        if (GridFitter.selectedSoulObject != this)
        {
            BlockInfoMenuHandler.InfoMenuHandler.Remove();
        }
    }

    public void ActivateEffect() {
        foreach (Effect effect in effects)
        {
            if (effect.targetType == TargetType.Self)
            {
                List<Fighter> l = new List<Fighter>();
                l.Add(Player.player);
                effect.targets = l;
                effect.ActivateEffect(Player.player);
            }
            else if (effect.targetType == TargetType.AllEnemies)
            {
                foreach (Enemy e in Battle.b.enemies)
                {
                    effect.targets.Add(e);
                }
                effect.ActivateEffect(Player.player);
            }
            else
            {
                effect.ActivateEffect(Player.player);
            }
        }
    }

    public void changeCooldownColor()
    {
        if (Player.player.stunned)
        {
            SetColor(originalColor * 0.1f);
            if (cooldownIndicator == null) // If there is no cooldown indicator then perform the following actions
            {
                cooldownIndicator = Instantiate(Resources.Load("Text") as GameObject, transform.position, Quaternion.identity, transform);

                TextMeshPro textSettings = cooldownIndicator.GetComponent<TextMeshPro>();
                textSettings.outlineColor = new Color(0, 0.679903f, 8301887f);
                textSettings.outlineWidth = 0.3f;
                textSettings.fontSize = 16;
            }

            TextMeshPro cooldownText = cooldownIndicator.GetComponent<TextMeshPro>();
            cooldownText.SetText("-");
        }
        else if (currentCooldown > 0)
        {
            SetColor(originalColor * 0.3f);
            if (cooldownIndicator == null) // If there is no cooldown indicator then perform the following actions
            {
                cooldownIndicator = Instantiate(Resources.Load("Text") as GameObject, transform.position, Quaternion.identity, transform);

                TextMeshPro textSettings = cooldownIndicator.GetComponent<TextMeshPro>();
                textSettings.outlineColor = new Color(0, 0.679903f, 8301887f);
                textSettings.outlineWidth = 0.3f;
                textSettings.fontSize = 16;
            }

            TextMeshPro cooldownText = cooldownIndicator.GetComponent<TextMeshPro>();
            cooldownText.SetText(currentCooldown.ToString());
        }
        else
        {
            SetColor(originalColor);
            if (currentCooldown == 0 && cooldownIndicator != null) // Destroys cooldown indicator once there is no cooldown
            { 
                Destroy(cooldownIndicator);
                cooldownIndicator = null;
            }
        }
    }

    public void cooldownStart()
    {
        currentCooldown = defaultCooldown;
    }

    public void SetRenderOrder(int order)
    {
        for (int i = 0; i < squareCount; i++)
        {
            GameObject sq = transform.GetChild(i).gameObject;
            sq.GetComponent<SpriteRenderer>().sortingOrder = order;
        }
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < squareCount; i++)
        {
            GameObject sq = transform.GetChild(i).gameObject;
            sq.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void SetActiveParticles(bool active)
    {
        for (int i = 0; i < squareCount; i++)
        {
            GameObject particle = transform.GetChild(i).GetChild(0).gameObject;
            particle.SetActive(active);
        }
    }

    public void showEffect()
    {
        GameObject p = Instantiate(Resources.Load<GameObject>("BlockUsageParticle"), transform.position, Quaternion.identity);
        ParticleSystem.MainModule m = p.GetComponent<ParticleSystem>().main;
        m.startColor = originalColor;
        ParticleSystem.ShapeModule s = p.GetComponent<ParticleSystem>().shape;
        s.radius = (width + height) / 4f;
    }

    public void CreateInfoMenu()
    {
        BlockInfoMenuHandler.InfoMenuHandler.Set(SoulNameWithoutUnderscores(), InfoText());
    }


    public string SoulNameWithoutUnderscores()
    {
        string[] soulNameParts = soulName.Split("_");
        string fixedName = "";
        for (int i = 0; i < soulNameParts.Length - 1; i++)
        {
            fixedName += soulNameParts[i] + " ";
        }
        fixedName += soulNameParts[soulNameParts.Length - 1];
        return fixedName;
    }

    public string InfoText()
    {
        string info = "Cooldown: " + currentCooldown + "/" + defaultCooldown + "\n";
        info += "Element: ";
        if (element == Element.Elements.FIRE)
        {
            info += "Fire";
        }
        else if (element == Element.Elements.WATER)
        {
            info += "Water";
        }
        else if (element == Element.Elements.NATURE)
        {
            info += "Nature";
        }
        else if (element == Element.Elements.ELEMENTLESS)
        {
            info += "Elementless";
        }
        info += "\nEffects:";
        foreach (Effect e in effects)
        {
            string s = Effect.effectToString(e, true);
            if (s != null)
            {
                info += "\n- " + s;
            }
        }
        return info;
    }
}
