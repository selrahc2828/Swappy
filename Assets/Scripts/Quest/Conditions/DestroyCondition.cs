using UnityEngine;

public class DestroyCondition : Condition
{
    private void OnDestroy()
    {
        SetConditionState(true);
    }
}
