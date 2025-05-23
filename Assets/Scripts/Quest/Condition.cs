using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Condition : MonoBehaviour
{
    private enum ConditionRedundancyType
    {
        OneOf,
        MultipleDiscrete,
        MultipleContinuous,
    }

    [SerializeField] private ConditionRedundancyType ConditionRedudancy = ConditionRedundancyType.OneOf;
    public UnityEvent<bool, Condition> ConditionOutput;
    [Space(16)]

    private bool hasBeenTrueOnce = false;
    private bool lastConditionState = false;

    protected void SetConditionState(bool state)
    {
        switch (ConditionRedudancy) 
        {
            case ConditionRedundancyType.OneOf:
                if (hasBeenTrueOnce) 
                {
                    return;
                }
                if (state)
                {
                    hasBeenTrueOnce = true;
                    ConditionOutput.Invoke(state, this);
                }
                return;
                
            case ConditionRedundancyType.MultipleDiscrete:
                if (lastConditionState == state)
                {
                    return;
                }
                lastConditionState = state;
                ConditionOutput.Invoke(state, this);
                return;

            case ConditionRedundancyType.MultipleContinuous:
                ConditionOutput.Invoke(state, this);
                return;

            default:
                return;
        }
    }

    private void OnDrawGizmos()
    {
        int listenersCount = ConditionOutput.GetPersistentEventCount();
        for (int i = 0; i < listenersCount; i++)
        {
            if (ConditionOutput.GetPersistentTarget(i))
            {
                Gizmos.DrawLine(GetConditionLineStart(), ConditionOutput.GetPersistentTarget(i).GetComponent<Transform>().position + Vector3.up * 2.5f);
                Gizmos.DrawCube(transform.position + Vector3.up, Vector3.one * 0.7f);
            }
        }
    }

    protected virtual Vector3 GetConditionLineStart()
    {
        return transform.position;
    }
}
