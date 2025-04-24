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

    public UnityEvent<bool, Condition> ConditionOutput;
    [SerializeField] private ConditionRedundancyType ConditionRedudancy;

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
        for (int i = 0; i <= listenersCount; i++)
        {
            Gizmos.DrawLine(GetConditionLineStart(), ConditionOutput.GetPersistentTarget(i).GetComponent<Transform>().position + Vector3.up * 2.5f);
            Gizmos.DrawCube(transform.position + Vector3.up, Vector3.one * 0.7f);
        }

    }

    public virtual Vector3 GetConditionLineStart()
    {
        return transform.position;
    }

    public virtual bool CheckObjectParameters(GameObject target)
    {
        Debug.LogError("La condition '" + this + "' dans l'objet " + transform.name + " ne peux pas être utilisée comme complément d'une autre condition !");
        return false;
    }
}
