using UnityEngine;

public class Condition : MonoBehaviour
{
    [Tooltip("La Quête remplie par cette condition. Si ce champs est vide, la condition sera considérée comme complément d'une autre condition")] 
    [SerializeField] protected Quest attachedQuest;
    [Tooltip("En règle générale, laisser l'état de base de la condition à false")] 
    [SerializeField] private bool defaultState = false;

    private void Start()
    {
        if (attachedQuest == null)
        {
            return;
        }

        attachedQuest.ReferenceCondition(this, defaultState);
    }

    protected virtual bool CheckObjectParameters(GameObject target)
    {
        Debug.LogError("La condition '" + this + "' dans l'objet " + transform.name + " ne peux pas être utilisée comme complément d'une autre condition !");
        return false;
    }

    public void OnDrawGizmos()
    {
        if (attachedQuest != null)
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

    protected virtual void OnDestroy()
    {

    }
}
