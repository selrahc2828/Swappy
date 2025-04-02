using UnityEngine;

public class TestCondition : QuestCondition
{
    void OnDestroy()
    {
        SetConditionState(true);
    }
}
