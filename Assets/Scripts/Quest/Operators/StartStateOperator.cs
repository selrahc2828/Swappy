using UnityEngine;

public class StartStateOperator : OperatorCondition
{
    [SerializeField] private bool startState;

    private void Start()
    {
        ConditionOutput.Invoke(startState);
    }

    public override void OperatorInput(bool state, Condition source)
    {
        SetConditionState(state);
    }
}