using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class representing a SoulObject placeable in the grid.
public class SoulObject : MonoBehaviour
{
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
    public SpriteRenderer soulRenderer;

    public bool isAoe;
    public bool isSingleTarget;
    public bool isHeal;
    public int damage;
    public int heal;

    public List<Effect> effects = new List<Effect>();
    public List<Fighter> targets;

    // Start is called before the first frame update
    void Start()
    {
        soulCollider = GetComponent<Collider2D>();
        soulRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        mouseTouching = true;
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
                effect.ActivateEffect(l);
            }
            else
            {
                effect.ActivateEffect(targets);
            }
        }
    }

}
