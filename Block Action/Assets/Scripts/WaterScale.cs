using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScale : Scale
{
    public WaterScale(Type t, Comparison c, float min, float max, float minScale, float maxScale) : base(t, c, min, max, minScale, maxScale)
    {

    }

    public override float Value(Fighter user, Fighter target)
    {
        if (type == Type.User)
        {
            if (user.currentElement == Element.Elements.WATER)
            {
                return user.currentElementStack;
            }
            return 0;
        }
        else
        {
            if (target.currentElement == Element.Elements.WATER)
            {
                return target.currentElementStack;
            }
            return 0;
        }
    }

    public override string ToString()
    {
        if (type == Type.User)
        {
            return "user's water stacks";
        }
        else
        {
            return "target's water stacks";
        }
    }
}
