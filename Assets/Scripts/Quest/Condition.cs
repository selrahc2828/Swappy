using UnityEngine;

public class Condition : MonoBehaviour
{
    [Tooltip("La Qu�te remplie par cette condition")] 
    [SerializeField] private Quest questScript;
    [Tooltip("En r�gle g�n�rale, laisser l'�tat de base de la condition � false")] 
    [SerializeField] private bool defaultState = false;
    [Tooltip("Activer UNIQUEMENT si ce script est utilis� comme compl�ment d'une autre condition")]
    [SerializeField] private bool isAdditionalCheck = false;

    private void Start()
    {
        if (questScript == null && isAdditionalCheck == false)
        {
            Debug.LogError("Il n'y a pas de script de Qu�te pr�cis� dans cette Condition !");
            return;
        }

        questScript.ReferenceThisCondition(this, defaultState);
    }

    protected void SetConditionState(bool state)
    {
        if (isAdditionalCheck)
        {
            Debug.LogWarning("Cette condition est marqu�e comme un AdditionalCheck et ne doit donc pas �tre reli�e � une qu�te");
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
