using UnityEngine;

public class InvertOperator : OperatorCondition
{
    public override void OperatorInput(bool state, Condition source)
    {
        SetConditionState(!state);
    }
}