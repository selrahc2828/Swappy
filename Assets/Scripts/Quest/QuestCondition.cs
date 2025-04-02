using UnityEngine;
using UnityEngine.Events;

public class QuestCondition : MonoBehaviour
{
    [Header("Reference")]

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
            Debug.LogError("Il n'y a pas de Gestionnaire de Qu�te pr�cis� dans cette Condition !");
            return;
        }

        questScript.ReferenceThisCondition(this, defaultState);
    }

    protected void SetConditionState(bool state)
    {
        if (isAdditionalCheck)
        {
            Debug.LogWarning("Cette condition est marqu�e comme un AdditionalCheck");
            return;
        }

        questScript.ChangeQuestConditions(this, state);
    }
}
