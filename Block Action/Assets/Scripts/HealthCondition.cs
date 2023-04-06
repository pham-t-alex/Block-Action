using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCondition : Condition
{
    double value;
    public HealthCondition(Type t, Comparison c, double v) : base(t, c)
    {
        value = v;
    }
    public override bool Fulfilled(Fighter user, Fighter target)
    {
        if (comparison == Comparison.Greater)
        {
            if (type == Type.User)
            {
                return user.health > value;
            }
            else if (type == Type.Target)
            {
                return target.health > value;
            }
            else
            {
                return user.health > target.health;
            }
        }
        else if (comparison == Comparison.Less)
        {
            if (type == Type.User)
            {
                return user.health < value;
            }
            else if (type == Type.Target)
            {
                return target.health < value;
            }
            else
            {
                return user.health < target.health;
            }
        }
        else if (comparison == Comparison.PercentageGreater)
        {
            if (type == Type.User)
            {
                return (user.health * 100 / user.maxHealth) > value;
            }
            else if (type == Type.Target)
            {
                return (target.health * 100 / target.maxHealth) > value;
            }
            else
            {
                return (user.health * 100 / user.maxHealth) > (target.health * 100 / target.maxHealth);
            }
        }
        else if (comparison == Comparison.PercentageLess)
        {
            if (type == Type.User)
            {
                return (user.health * 100 / user.maxHealth) < value;
            }
            else if (type == Type.Target)
            {
                return (target.health * 100 / target.maxHealth) < value;
            }
            else
            {
                return (user.health * 100 / user.maxHealth) < (target.health * 100 / target.maxHealth);
            }
        }
        else if (comparison == Comparison.PercentageEquality)
        {
            if (type == Type.User)
            {
                return (user.health * 100 / user.maxHealth) == value;
            }
            else if (type == Type.Target)
            {
                return (target.health * 100 / target.maxHealth) == value;
            }
            else
            {
                return (user.health * 100 / user.maxHealth) == (target.health * 100 / target.maxHealth);
            }
        }
        else
        {
            if (type == Type.User)
            {
                return user.health == value;
            }
            else if (type == Type.Target)
            {
                return target.health == value;
            }
            else
            {
                return user.health == target.health;
            }
        }
    }

    public override string ToString()
    {
        string s = "If ";
        if (type == Type.Target)
        {
            s += "target's health ";
        }
        else
        {
            s += "user's health ";
        }
        if (comparison == Comparison.Greater)
        {
            s += "> ";
        }
        else if (comparison == Comparison.Less)
        {
            s += "< ";
        }
        else if (comparison == Comparison.PercentageGreater)
        {
            s += "% > ";
        }
        else if (comparison == Comparison.PercentageLess)
        {
            s += "% < ";
        }
        else if (comparison == Comparison.PercentageEquality)
        {
            s += "% = ";
        }
        else
        {
            s += "= ";
        }
        if (type == Type.Comparative)
        {
            s += "target's health";
            if (comparison == Comparison.PercentageGreater || comparison == Comparison.PercentageLess || comparison == Comparison.PercentageEquality)
            {
                s += " %";
            }
        }
        else
        {
            s += value;
            if (comparison == Comparison.PercentageGreater || comparison == Comparison.PercentageLess || comparison == Comparison.PercentageEquality)
            {
                s += "%";
            }
        }
        s += ": ";
        return s;
    }
}