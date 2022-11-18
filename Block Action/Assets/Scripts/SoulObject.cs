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

    public bool isAoe;
    public bool isSingleTarget;
    public bool isHeal;
    public int damage;
    public int heal;

    // Variables required for cooldown calculations
    public int defaultCooldown;
    public int currentCooldown;

    // Variable for initiating cooldown text
    public GameObject cooldownIndicator = null;
    public int layer;

    // Variable used for changing soul color when on cooldown
    private SpriteRenderer _spriteRenderer;

    public List<Effect> effects = new List<Effect>();
    public List<Fighter> targets;

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

    void OnMouseEnter()
    {
        if (currentCooldown == 0) // If block is on cooldown, disable mouse interaction
        {
            mouseTouching = true;
        }
    }

    void OnMouseExit()
    {
        mouseTouching = false;
    }

    public void ActivateEffect() {
        foreach (Effect effect in effects)
        {
            if (effect.self)
            {
                List<Fighter> l = new List<Fighter>();
                l.Add(Player.player);
                effect.targets = l;
                effect.ActivateEffect(Player.player);
            }
            else
            {
                effect.targets = targets;
                effect.ActivateEffect(Player.player);
            }
        }
    }

    public void changeCooldownColor()
    {
        if (currentCooldown > 0)
        {
            SetColor(originalColor * 0.3f);
            SetActiveParticles(false);
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
            SetActiveParticles(true);
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
}
