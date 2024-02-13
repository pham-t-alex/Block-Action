using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalApplicationEffect : Effect
{
    public Element.Elements element;
    public int stackCount;

    public ElementalApplicationEffect(Element.Elements element, int stackCount)
    {
        this.element = element;
        this.stackCount = stackCount;
    }

    public override void ActivateEffect(Fighter fighter)
    {
        foreach (Fighter f in targets)
        {
            if (!f.dead)
            {
                ApplyElementalStack(f, element, stackCount);
                string elementString = "";
                switch (element)
                {
                    case Element.Elements.FIRE:
                        elementString = "Fire";
                        break;
                    case Element.Elements.NATURE:
                        elementString = "Nature";
                        break;
                    case Element.Elements.WATER:
                        elementString = "Water";
                        break;
                    case Element.Elements.ELEMENTLESS:
                        elementString = "Elementless";
                        break;
                }
                elementString += " x" + stackCount;
                if (f.Equals(Player.player))
                {
                    Debug.Log(elementString + " applied to the player");
                }
                else
                {
                    Debug.Log(elementString + " applied to an enemy");
                }
            }
        }
    }

    //Assumes stackCount is positive
    public static void ApplyElementalStack(Fighter f, Element.Elements element, int stackCount)
    {
        int weakness = Element.weaknessCheck(element, f.currentElement);
        if (weakness == 1)
        {
            if (2 * stackCount > f.currentElementStack)
            {
                stackCount -= (int) Mathf.Ceil(f.currentElementStack / 2.0f);
                f.currentElementStack = stackCount;
                f.currentElement = element;
            }
            else if (2 * stackCount < f.currentElementStack)
            {
                f.currentElementStack -= 2 * stackCount;
            }
            else
            {
                f.currentElementStack = 0;
                f.currentElement = Element.Elements.ELEMENTLESS;
            }
        }
        else if (weakness == -1)
        {
            if (0.5 * stackCount > f.currentElementStack)
            {
                stackCount -= f.currentElementStack * 2;
                f.currentElementStack = stackCount;
                f.currentElement = element;
            }
            else if (0.5 * stackCount < f.currentElementStack)
            {
                f.currentElementStack -= (int) (0.5f * stackCount);
            }
            else
            {
                f.currentElementStack = 0;
                f.currentElement = Element.Elements.ELEMENTLESS;
            }
        }
        else
        {
            if (f.currentElement == element)
            {
                f.currentElementStack += stackCount;
            }
            else
            {
                if (stackCount > f.currentElementStack)
                {
                    stackCount -= f.currentElementStack;
                    f.currentElementStack = stackCount;
                    f.currentElement = element;
                }
                else if (stackCount < f.currentElementStack)
                {
                    f.currentElementStack -= stackCount;
                }
                else
                {
                    f.currentElementStack = 0;
                    f.currentElement = Element.Elements.ELEMENTLESS;
                }
            }
        }
        if (f.currentElementStack > Element.MAX_ELEMENT_STACK)
        {
            f.currentElementStack = Element.MAX_ELEMENT_STACK;
        }
        if (f.currentElement == Element.Elements.ELEMENTLESS)
        {
            f.currentElementStack = 0;
        }
        else if (f.currentElementStack == 0)
        {
            f.currentElement = Element.Elements.ELEMENTLESS;
        }
    }
}