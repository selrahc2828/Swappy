using UnityEngine;

public class DestroyCondition : Condition
{
    [Space(16)]
    [SerializeField] private bool onDestroyState = true;

    private void OnDestroy()
    {
        SetConditionState(onDestroyState);
    }
}
