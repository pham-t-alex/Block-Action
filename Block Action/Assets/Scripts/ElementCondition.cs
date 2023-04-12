using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCondition : Condition
{
    Element.Elements element;
    public ElementCondition(Type t, Comparison c, Element.Elements e) : base(t, c)
    {
        element = e;
    }
    public override bool Fulfilled(Fighter user, Fighter target)
    {
        if (type == Type.User)
        {
            return user.currentElement == element;
        }
        else if (type == Type.Target)
        {
            return target.currentElement == element;
        }
        else
        {
            return user.currentElement == target.currentElement;
        }
    }

    public override string ToString()
    {
        string s = "If ";
        if (type == Type.Target)
        {
            s += "target's element ";
        }
        else
        {
            s += "user's element ";
        }
        s += "= ";
        if (type == Type.Comparative)
        {
            s += "target's element";
        }
        else
        {
            switch (element)
            {
                case Element.Elements.FIRE:
                    s += "Fire";
                    break;
                case Element.Elements.WATER:
                    s += "Water";
                    break;
                case Element.Elements.NATURE:
                    s += "Nature";
                    break;
                case Element.Elements.ELEMENTLESS:
                    s += "Elementless";
                    break;
            }
        }
        s += ": ";
        return s;
    }
}