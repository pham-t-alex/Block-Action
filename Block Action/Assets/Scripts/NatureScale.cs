using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureScale : Scale
{
    public NatureScale(Type t, Comparison c, float min, float max, float minScale, float maxScale) : base(t, c, min, max, minScale, maxScale)
    {

    }

    public override float Value(Fighter user, Fighter target)
    {
        if (type == Type.User)
        {
            if (user.currentElement == Element.Elements.NATURE)
            {
                return user.currentElementStack;
            }
            return 0;
        }
        else
        {
            if (target.currentElement == Element.Elements.NATURE)
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
            return "user's nature stacks";
        }
        else
        {
            return "target's nature stacks";
        }
    }
}
