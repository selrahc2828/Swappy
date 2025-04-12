using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum QuestTypes
{
    NPCQuest,
    LoreQuest,
    Collectible,
    BlankActivity
}
public enum QuestRedundancy
{
    OneOf,
    Multiple
}
public class Quest : MonoBehaviour
{
    public string QuestName;
    public QuestTypes QuestType;
    public QuestRedundancy Redundancy;
    public UnityEvent QuestEvent;

    public readonly Dictionary<QuestTypes, Color> QuestColors = new Dictionary<QuestTypes, Color>()
    {   {QuestTypes.NPCQuest, Color.red},
        {QuestTypes.LoreQuest, Color.blue},
        {QuestTypes.Collectible, Color.magenta},
        {QuestTypes.BlankActivity, Color.green} };

    private Dictionary<Condition, bool> ActiveConditions = new Dictionary<Condition, bool>();


    public void ReferenceCondition(Condition conditionScript, bool state) //R�f�rence toutes les conditions d'une qu�te � l'initialisation dans un dictionnaire
    {
        ActiveConditions.Add(conditionScript, state);
    }

    public void SetCondition(Condition condition, bool state) //Change le state d'une des condition de la qu�te
    {
        if (ActiveConditions[condition] == state)
        {
            //bloque toute tentative d'update une quest condition avec la m�me valeur, �vitant de call plusieurs fois un accomplissement de qu�te.
            return;
        }

        ActiveConditions[condition] = state;

        CheckConditions();
    }

    private void CheckConditions() //V�rifie si la qu�te est accomplie en parcourant toutes les entr�es du dictionnaire et en regardant si l'une d'elle est false
    {
        bool debugPreBool = true;

        foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
        {
            Debug.Log("ActiveCondition: " + conditionRef.Key + " | " + conditionRef.Value);

            if (conditionRef.Value == false)
            {
                debugPreBool = false;
            }
        }

        if (debugPreBool == false)
        {
            return;
        }

        Debug.Log("Quest '" + QuestName + "' accomplished");
        QuestEvent.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (QuestColors.TryGetValue(QuestType, out Color questColor))
        {
            Gizmos.color = questColor;
        }

        Gizmos.DrawSphere(transform.position + Vector3.up * 2.5f, 0.8f);
    }
}


