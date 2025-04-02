using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCondition : QuestCondition
{
    public GameObject TestObject;

    private void OnTestObjectDestroy()
    {
        SetConditionBool.Invoke();
    }
}
