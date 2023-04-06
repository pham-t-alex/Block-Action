using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    //Need to type element name in all caps btw
    public enum Elements{ELEMENTLESS, FIRE, WATER, NATURE};

    //Return -1 if the enemy resisted the attack, 0 if it's normal damage,
    //and 1 if the one taking damage was weak to the attack
    private static int weaknessCheck(Elements attackElement, Elements damagedElement) {
        //A perk of choosing elementless attacks could be that the attack will never be resisted
        int weaknessValue = 0;
        
        if (attackElement == Elements.FIRE)
        {
            //Fire type enemies will take less damage from fire moves
            if (damagedElement == Elements.FIRE) 
            {
                weaknessValue = -1;
            } 
            //Nature is weak to fire
            else if (damagedElement == Elements.NATURE) 
            {
                weaknessValue = 1;
            }
        }
        else if (attackElement == Elements.WATER)
        {
            //Water resists water
            if (damagedElement == Elements.WATER)
            {
                weaknessValue = -1;
            }
            //Water beats fire
            if (damagedElement == Elements.FIRE)
            {
                weaknessValue = 1;
            }
        }
        else if (attackElement == Elements.NATURE)
        {
            //Nature resists itself
            if (damagedElement == Elements.NATURE)
            {
                weaknessValue = -1;
            }
            //Nature beats water? Probably not gonna be a set-in-stone thing but-
            if (damagedElement == Elements.WATER)
            {
                weaknessValue = 1;
            }
        }


        return weaknessValue;
    }


    //I'm basically assuming that the player is not going to have an element so all attacks will do a set damage to them
    //so the first parameter will be the element the player is attacking with and the second is the enemy's element
    public static float elementalDamageModifier(Elements playerElement, Elements enemyElement)
    {
        int weakness = Element.weaknessCheck(playerElement, enemyElement);
        
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
