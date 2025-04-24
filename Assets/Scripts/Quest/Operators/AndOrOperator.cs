using System.Collections.Generic;
using UnityEngine;

public class AndOrOperator : OperatorCondition
{
    public enum OperatorTypes
    {
        And,
        Or
    }

    [SerializeField] private OperatorTypes operatorType;
    [SerializeField] private int argumentCount;

    private Dictionary<Condition, bool> ActiveConditions = new Dictionary<Condition, bool>();

    public override void OperatorInput(bool state, Condition source)
    {
        switch (operatorType)
        {
            case OperatorTypes.And:
                if (argumentCount != ActiveConditions.Count)
                {
                    SetConditionState(false);
                    return;
                }
                foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
                {
                    if (conditionRef.Value == false)
                    {
                        SetConditionState(true);
                        return;
                    }
                }
                SetConditionState(false);
                return;

            case OperatorTypes.Or:
                foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
                {
                    if (conditionRef.Value == true)
                    {
                        SetConditionState(true);
                        return;
                    }
                }
                SetConditionState(false);
                return;

            default:
                SetConditionState(false);
                return;
        }
    }
}
