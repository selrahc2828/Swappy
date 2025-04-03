using System.Collections.Generic;
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
        if (isAdditionalCheck)
        {
            return;
        }

        if (attachedQuest == null)
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

    protected virtual bool CheckObjectParameters(GameObject target)
    {
        Debug.LogError("La condition '" + this + "' dans l'objet " + transform.name + " ne peux pas être utilisée comme complément d'une autre condition !");
        return false;
    }

    public void OnDrawGizmos()
    {
        if (attachedQuest != null && isAdditionalCheck == false)
        {
            if (attachedQuest.QuestColors.TryGetValue(attachedQuest.QuestType, out Color questColor))
            {
                Gizmos.color = questColor;
            }

            Gizmos.DrawLine(GetQuestLineStart(), attachedQuest.transform.position + Vector3.up * 2.5f);
            Gizmos.DrawCube(transform.position + Vector3.up, Vector3.one * 0.7f);
        }

    }

    protected virtual Vector3 GetQuestLineStart()
    {
        return transform.position;
    }
}
