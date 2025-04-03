using UnityEngine;

public class QuestCondition : Condition
{
    [Space(16)]
    [SerializeField] private Quest receivingQuest;

    private void Start()
    {
        receivingQuest.QuestEvent.AddListener(SetQuestCondition);
    }

    private void SetQuestCondition()
    {
        SetConditionState(true);
    }
}
