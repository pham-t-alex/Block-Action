using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //Need to type element name in all caps btw
    public enum Elements{ELEMENTLESS, FIRE, WATER, NATURE};
    public Elements playerElement;


    public Element(Elements type)
    {
        playerElement = type;
    }

    //Return -1 if the enemy resisted the attack, 0 if it's normal damage,
    //and 1 if the enemy was weak to the attack
    private int weaknessCheck(Elements enemyType) {
        //A perk of choosing elementless attacks could be that the attack will never be resisted
        int weaknessValue = 0;
        
        if (playerElement == Elements.FIRE)
        {
            //Fire type enemies will take less damage from fire moves
            if (enemyType == Elements.FIRE) 
            {
                weaknessValue = -1;
            } 
            //Nature is weak to fire
            else if (enemyType == Elements.NATURE) 
            {
                weaknessValue = 1;
            }
        }
        else if (playerElement == Elements.WATER)
        {
            //Water resists water
            if (enemyType == Elements.WATER)
            {
                weaknessValue = -1;
            }
            //Water beats fire
            if (enemyType == Elements.FIRE)
            {
                weaknessValue = 1;
            }
        }
        else if (playerElement == Elements.NATURE)
        {
            //Nature resists itself
            if (enemyType == Elements.NATURE)
            {
                weaknessValue = -1;
            }
            //Nature beats water? Probably not gonna be a set-in-stone thing but-
            if (enemyType == Elements.WATER)
            {
                weaknessValue = 1;
            }
        }


        return weaknessValue;
    }

    public float elementalDamageModifier(Elements enemyElement)
    {
        int weakness = this.weaknessCheck(enemyElement);
        
        //dmgModifier is the number that the attack damage will be multiplied by
        //depending on the element of the attack and enemmy
        float dmgModifier = 0.0f;
        if (weakness == -1)
        {
            dmgModifier = 0.5f;
        }
        else if (weakness == 0)
        {
            dmgModifier = 1.0f;
        }
        else if (weakness == 1)
        {
            dmgModifier = 2.0f;
        }

        return dmgModifier;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
