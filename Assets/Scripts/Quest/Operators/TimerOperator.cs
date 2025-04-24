using UnityEngine;

public class TimerOperator : OperatorCondition
{
    public enum OperatorTypes
    {
        Wait,
        WaitStrict,
        Invert,
        InvertStrict
    }

    public override void OperatorInput(bool state, Condition source)
    {

    }
}