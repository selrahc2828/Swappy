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

    public override bool OperatorInput(bool state, Condition source)
    {
        switch (operatorType)
        {
            case OperatorTypes.And:
                if (argumentCount != ActiveConditions.Count)
                {
                    return false;
                }
                foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
                {
                    if (conditionRef.Value == false)
                    {
                        return false;
                    }
                }
                return true;

            case OperatorTypes.Or:
                foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
                {
                    if (conditionRef.Value == true)
                    {
                        return true;
                    }
                }
                return false;

                default: 
                    return false;
        }
    }
}
