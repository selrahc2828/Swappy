using UnityEngine;

public class TestCondition : Condition
{
    void OnDestroy()
    {
        SetConditionState(true);
    }
}
