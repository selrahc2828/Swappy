using UnityEngine;

public class QuestCondition : Condition
{
    [Space(16)]
    [SerializeField] private Quest receivingQuest;

    private void Start()
    {
        receivingQuest.QuestEvent.AddListener(ChangeQuestCondition);
    }

    private void ChangeQuestCondition()
    {
        attachedQuest.SetCondition(this, true);
    }

    protected override Vector3 GetQuestLineStart()
    {
        return receivingQuest.transform.position + Vector3.up * 2.5f;
    }
}
