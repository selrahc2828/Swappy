using UnityEngine;

public class DestroyCondition : Condition
{
    [Space(16)]
    [SerializeField] private bool onDestroyStateChange = true;

    private void OnDestroy()
    {
        SetConditionState(onDestroyStateChange);
    }
}
