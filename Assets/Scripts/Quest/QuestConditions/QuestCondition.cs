using UnityEngine;

public class QuestCondition : Condition
{
    [Space(16)]
    [SerializeField] private Quest receivingQuest;

    private void Start()
    {
        if (receivingQuest == attachedQuest)
        {
            Debug.LogError("La quête analysée doit être différente de la quête attachée !");
            return;
        }
        receivingQuest.QuestEvent.AddListener(ChangeQuestCondition);
    }

    private void ChangeQuestCondition()
    {
        attachedQuest.SetCondition(this, true);
    }

    protected override Vector3 GetConditionLineStart()
    {
        return receivingQuest.transform.position + Vector3.up * 2.5f;
    }
}
