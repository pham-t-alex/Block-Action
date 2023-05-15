using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScale : Scale
{
    public HealthScale(Type t, Comparison c, float min, float max, float minScale, float maxScale) : base(t, c, min, max, minScale, maxScale)
    {

    }

    public override float Value(Fighter user, Fighter target)
    {
        if (comparison == Comparison.Value)
        {
            if (type == Type.User)
            {
                return user.health;
            }
            else
            {
                return target.health;
            }
        }
        else
        {
            if (type == Type.User)
            {
                return (100f * user.health) / user.maxHealth;
            }
            else
            {
                return (100f * target.health) / target.maxHealth;
            }
        }
    }

    public override string ToString()
    {
        if (type == Type.User)
        {
            return "user's health";
        }
        else
        {
            return "target's health";
        }
    }
}
