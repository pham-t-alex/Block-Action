using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCondition : Condition
{
    Condition firstCondition;
    Condition secondCondition;
    public AndCondition(Condition one, Condition two) : base(Type.User, Comparison.Equality)
    {
        firstCondition = one;
        secondCondition = two;
    }
    public override bool Fulfilled(Fighter user, Fighter target)
    {
        return firstCondition.Fulfilled(user, target) && secondCondition.Fulfilled(user, target);
    }

    public override string ToString()
    {
        throw new System.NotImplementedException();
    }
}
