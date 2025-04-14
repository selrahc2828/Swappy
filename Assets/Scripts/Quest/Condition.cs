using UnityEngine;

public class Condition : MonoBehaviour
{
    [Tooltip("La Qu�te remplie par cette condition. Si ce champs est vide, la condition sera consid�r�e comme compl�ment d'une autre condition")] 
    [SerializeField] protected Quest attachedQuest;
    [Tooltip("En r�gle g�n�rale, laisser l'�tat de base de la condition � false")] 
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
        Debug.LogError("La condition '" + this + "' dans l'objet " + transform.name + " ne peux pas �tre utilis�e comme compl�ment d'une autre condition !");
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
