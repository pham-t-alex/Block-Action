using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScale : Scale
{
    public FireScale(Type t, Comparison c, float min, float max, float minScale, float maxScale) : base(t, c, min, max, minScale, maxScale)
    {

    }

    public override float Value(Fighter user, Fighter target)
    {
        if (type == Type.User)
        {
            if (user.currentElement == Element.Elements.FIRE)
            {
                return user.currentElementStack;
            }
            return 0;
        }
        else
        {
            if (target.currentElement == Element.Elements.FIRE)
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
            return "user's fire stacks";
        }
        else
        {
            return "target's fire stacks";
        }
    }
}
