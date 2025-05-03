using UnityEngine;

public class CountOperator : OperatorCondition
{
    private enum OperatorTypes
    {
        Precise,
        MinMax
    }

    [SerializeField] private OperatorTypes operatorType;
    [SerializeField] private bool isFalseReducingCounter;
    [SerializeField] private int preciseCount;
    [SerializeField] private int minCount;
    [SerializeField] private int maxCount;

    private int counter;

    public override void OperatorInput(bool state, Condition source)
    {
        if (state)
        {
            counter++;
        }
        else if (isFalseReducingCounter)
        {
            counter--;
        }

        switch (operatorType)
        {
            case OperatorTypes.Precise:
               if (counter == preciseCount) 
               {
                    SetConditionState(true);
                    return;
               }
               SetConditionState(false);
               return;

            case OperatorTypes.MinMax:
                if (counter >= minCount && counter <= maxCount) 
                {
                    SetConditionState(true);
                    return;
                }
                SetConditionState(false);
                return;
        }
    }
}
