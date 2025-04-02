using UnityEngine;
using UnityEngine.Events;

public class QuestCondition : MonoBehaviour
{
    [Header("Reference")]

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
            Debug.LogError("Il n'y a pas de Gestionnaire de Quête précisé dans cette Condition !");
            return;
        }

        questScript.ReferenceThisCondition(this, defaultState);
    }

    protected void SetConditionState(bool state)
    {
        if (isAdditionalCheck)
        {
            Debug.LogWarning("Cette condition est marquée comme un AdditionalCheck");
            return;
        }

        questScript.ChangeQuestConditions(this, state);
    }
}
