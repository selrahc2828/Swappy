using UnityEngine;

public class QuestCondition : Condition
{
    [Space(16)]
    [SerializeField] private Quest receivingQuest;

    private void Start()
    {
        if (receivingQuest == attachedQuest)
        {
            Debug.LogError("La qu�te analys�e doit �tre diff�rente de la qu�te attach�e !");
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
