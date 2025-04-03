using UnityEngine;

public class Condition : MonoBehaviour
{
    [Tooltip("La Quête remplie par cette condition")] 
    [SerializeField] private Quest questScript;
    [Tooltip("En règle générale, laisser l'état de base de la condition à false")] 
    [SerializeField] private bool defaultState = false;
    [Tooltip("Activer UNIQUEMENT si ce script est utilisé comme complément d'une autre condition")]
    [SerializeField] private bool isAdditionalCheck = false;

    private void Start()
    {
        if (questScript == null && isAdditionalCheck == false)
        {
            Debug.LogError("Il n'y a pas de script de Quête précisé dans cette Condition !");
            return;
        }

        questScript.ReferenceThisCondition(this, defaultState);
    }

    protected void SetConditionState(bool state)
    {
        if (isAdditionalCheck)
        {
            Debug.LogWarning("Cette condition est marquée comme un AdditionalCheck et ne doit donc pas être reliée à une quête");
            return;
        }

        questScript.ChangeQuestConditions(this, state);
    }

    private void OnDrawGizmos()
    {
        if (questScript != null && isAdditionalCheck == false)
        {
            if (questScript.QuestColors.TryGetValue(questScript.QuestType, out Color questColor))
            {
                Gizmos.color = questColor;
            }
            
            Gizmos.DrawLine(transform.position, questScript.transform.position + Vector3.up * 2);
        }

    }
}
