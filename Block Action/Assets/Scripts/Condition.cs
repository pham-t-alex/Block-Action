using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition
{
    public enum Type
    {
        Comparative, //compares user vs target
        User, //compares user to value
        Target //compares target to value
    }
    public Type type;
    public enum Comparison
    {
        Equality, //exact equality
        Greater, //exact greater
        Less, //exact less
        PercentageEquality,
        PercentageGreater,
        PercentageLess,
    }
    public Comparison comparison;

    public Condition(Type type, Comparison comparison)
    {
        this.type = type;
        this.comparison = comparison;
    }

    public abstract bool Fulfilled(Fighter user, Fighter target);

    public static Condition conditionFromStrings(string subclass, string comparison, string object1, string object2)
    {
        //example conditional: "health" "%<" "user" "target"
        Condition condition = null;
        Comparison c;
        Type t;
        if (comparison.Equals("<"))
        {
            c = Comparison.Less;
        }
        else if (comparison.Equals(">"))
        {
            c = Comparison.Greater;
        }
        else if (comparison.Equals("%<"))
        {
            c = Comparison.PercentageLess;
        }
        else if (comparison.Equals("%>"))
        {
            c = Comparison.PercentageGreater;
        }
        else if (comparison.Equals("%="))
        {
            c = Comparison.PercentageEquality;
        }
        else
        {
            c = Comparison.Equality;
        }
        if (object1.Equals("target"))
        {
            t = Type.Target;
        }
        else
        {
            if (object2.Equals("target"))
            {
                t = Type.Comparative;
            }
            else
            {
                t = Type.User;
            }
        }
        if (subclass.Equals("health"))
        {
            if (t == Type.Comparative)
            {
                condition = new HealthCondition(t, c, 0);
            }
            else
            {
                condition = new HealthCondition(t, c, System.Convert.ToInt32(object2));
            }
        }
        else if (subclass.Equals("element"))
        {
            if (t == Type.Comparative)
            {
                condition = new ElementCondition(t, c, Element.Elements.ELEMENTLESS);
            }
            else
            {
                Element.Elements e;
                switch (object2)
                {
                    case "fire":
                        e = Element.Elements.FIRE;
                        break;
                    case "water":
                        e = Element.Elements.WATER;
                        break;
                    case "nature":
                        e = Element.Elements.NATURE;
                        break;
                    default:
                        e = Element.Elements.ELEMENTLESS;
                        break;
                }
                condition = new ElementCondition(t, c, e);
            }
        }
        return condition;
    }

    public abstract override string ToString();
}
