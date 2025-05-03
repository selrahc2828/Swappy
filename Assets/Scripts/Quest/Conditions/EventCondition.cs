using UnityEngine;

public class EventCondition : Condition
{
    [Space(16)]
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
