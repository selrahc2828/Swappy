using UnityEngine;

public class EventCondition : Condition
{
    [Tooltip("Uniquement utile pour donner une destination aux lignes de quêtes debug")]
    [SerializeField] private GameObject eventSource;

    public void SetEventConditionState(bool state)
    {
        SetConditionState(state);
    }

    protected override Vector3 GetConditionLineStart()
    {
       
        return eventSource.transform.position;
    }
}
