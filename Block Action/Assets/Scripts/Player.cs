using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private static Player _player;
    public static Player player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
            }
            return _player;
        }
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        health = 100; //might not be valid
        buff = 1.0;
        defenseBuff = 1.0;
        maxHealth = 100;
        baseElement = Element.Elements.ELEMENTLESS;
        currentElement = baseElement;
        currentElementStack = 0;
        statusEffects = new List<Status>();
        stunChargeMax = 100;


        Battle.b.fighters.Add(this);
        fadeOnDefeat = true;
    }

    public override string GetName()
    {
        return "Protagonist";
    }

    public override string GetInfo()
    {
        string info = "Health: " + health + "/" + maxHealth + "\n";
        info += "Element: ";
        if (currentElement == Element.Elements.FIRE)
        {
            info += "Fire x" + currentElementStack;
        }
        else if (currentElement == Element.Elements.WATER)
        {
            info += "Water x" + currentElementStack;
        }
        else if (currentElement == Element.Elements.NATURE)
        {
            info += "Nature x" + currentElementStack;
        }
        else if (currentElement == Element.Elements.ELEMENTLESS)
        {
            info += "Elementless";
        }
        if (baseElement == currentElement)
        {
            info += "\n";
        }
        else
        {
            info += " | Base: ";
            if (baseElement == Element.Elements.FIRE)
            {
                info += "Fire\n";
            }
            else if (baseElement == Element.Elements.WATER)
            {
                info += "Water\n";
            }
            else if (baseElement == Element.Elements.NATURE)
            {
                info += "Nature\n";
            }
            else if (baseElement == Element.Elements.ELEMENTLESS)
            {
                info += "Elementless\n";
            }
        }
        info += "Stun Charge: " + stunCharge + "/" + stunChargeMax + "\n";
        info += "Status Effects:";
        foreach (Status status in statusEffects)
        {
            string s = Status.statusToString(status);
            if (s != null)
            {
                info += "\n- " + s + ".";
            }
        }
        return info;
    }

    public void OnMouseExit()
    {
        DestroyInfoMenu();
        timeHovered = 0;
    }
}
