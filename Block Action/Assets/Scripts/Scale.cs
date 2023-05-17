using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scale
{
    public enum Type
    {
        User,
        Target
    }
    public Type type;

    public enum Comparison
    {
        Value,
        Percentage
    }
    public Comparison comparison;

    public float min;
    public float max;
    public float minScale;
    public float maxScale;

    public Scale(Type type, Comparison comparison, float min, float max, float minScale, float maxScale)
    {
        this.type = type;
        this.comparison = comparison;
        this.min = min;
        this.max = max;
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    public float ScaledValue(Fighter user, Fighter target)
    {
        float value = Value(user, target);
        if (min > max)
        {
            if (value >= min)
            {
                return minScale;
            }
            else if (value <= max)
            {
                return maxScale;
            }
            float proportion = (min - value) / (min - max);
            return minScale + (proportion * (maxScale - minScale));
        }
        else
        {
            if (value <= min)
            {
                return minScale;
            }
            else if (value >= max)
            {
                return maxScale;
            }
            float proportion = (value - min) / (max - min);
            return minScale + (proportion * (maxScale - minScale));
        }
    }

    public abstract float Value(Fighter user, Fighter target);

    public static Scale scaleFromData(string subclass, string type, string comparison, float min, float max, float minScale, float maxScale)
    {
        //example scale: "health" "user" "percentage" 0 100 1 4
        Scale scale = null;
        Comparison c;
        Type t;
        if (type.Equals("target"))
        {
            t = Type.Target;
        }
        else
        {
            t = Type.User;
        }
        if (comparison.Equals("percentage"))
        {
            c = Comparison.Percentage;
        }
        else
        {
            c = Comparison.Value;
        }
        if (subclass.Equals("health"))
        {
            scale = new HealthScale(t, c, min, max, minScale, maxScale);
        }
        else if (subclass.Equals("fire"))
        {
            scale = new FireScale(t, c, min, max, minScale, maxScale);
        }
        else if (subclass.Equals("water"))
        {
            scale = new WaterScale(t, c, min, max, minScale, maxScale);
        }
        else if (subclass.Equals("nature"))
        {
            scale = new NatureScale(t, c, min, max, minScale, maxScale);
        }
        return scale;
    }

    public abstract override string ToString();
}
