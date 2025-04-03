using UnityEngine;

public class QuestCondition : Condition
{
    [Space(16)]
    [SerializeField] private Quest targetQuestScript;

    private void Start()
    {
        targetQuestScript.QuestEvent.AddListener(SetQuestCondition);
    }

    private void SetQuestCondition()
    {
        SetConditionState(true);
    }
}
