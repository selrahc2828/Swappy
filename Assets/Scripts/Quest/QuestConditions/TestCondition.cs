using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCondition : QuestCondition
{
    void OnDestroy()
    {
        SetConditionState(true);
    }
}
