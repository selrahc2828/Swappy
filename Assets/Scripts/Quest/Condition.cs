using UnityEngine;

public class Condition : MonoBehaviour
{
    [Tooltip("La Quête remplie par cette condition")] 
    [SerializeField] private Quest attachedQuest;
    [Tooltip("En règle générale, laisser l'état de base de la condition à false")] 
    [SerializeField] private bool defaultState = false;
    [Tooltip("Activer UNIQUEMENT si ce script est utilisé comme complément d'une autre condition")]
    [SerializeField] private bool isAdditionalCheck = false;

    private void Start()
    {
        if (attachedQuest == null && isAdditionalCheck == false)
        {
            Debug.LogError("Il n'y a pas de script de Quête précisé dans cette Condition !");
            return;
        }

        attachedQuest.ReferenceThisCondition(this, defaultState);
    }

    protected void SetConditionState(bool state)
    {
        if (isAdditionalCheck)
        {
            Debug.LogWarning("Cette condition est marquée comme un AdditionalCheck et ne doit donc pas être reliée à une quête");
            return;
        }

        attachedQuest.ChangeQuestConditions(this, state);
    }

    private void OnDrawGizmos()
    {
        if (attachedQuest != null && isAdditionalCheck == false)
        {
            if (attachedQuest.QuestColors.TryGetValue(attachedQuest.QuestType, out Color questColor))
            {
                Gizmos.color = questColor;
            }
            
            Gizmos.DrawLine(transform.position, attachedQuest.transform.position + Vector3.up * 2);
        }

    }
}
