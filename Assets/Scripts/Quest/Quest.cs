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

public class Quest : MonoBehaviour
{
    public string QuestName;
    public QuestTypes QuestType;
    public UnityEvent QuestEvent;

    public readonly Dictionary<QuestTypes, Color> QuestColors = new Dictionary<QuestTypes, Color>()
    {   {QuestTypes.NPCQuest, Color.red},
        {QuestTypes.LoreQuest, Color.blue},
        {QuestTypes.Collectible, Color.magenta},
        {QuestTypes.BlankActivity, Color.green} };

    private Dictionary<Condition, bool> ActiveConditions = new Dictionary<Condition, bool>();


    public void ReferenceThisCondition(Condition conditionScript, bool state) //R�f�rence toutes les conditions d'une qu�te � l'initialisation dans un dictionnaire
    {
        ActiveConditions.Add(conditionScript, state);
    }

    public void ChangeQuestConditions(Condition condition, bool state) //Change le state d'une des condition de la qu�te
    {
        ActiveConditions[condition] = state;
        CheckQuestConditions();
    }

    private void CheckQuestConditions() //V�rifie si la qu�te est accomplie en parcourant toutes les entr�es du dictionnaires et en regardant si l'une d'elle est false
    {
        foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
        {
            if (conditionRef.Value == false)
            {
                return;  
            }
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

        Gizmos.DrawSphere(transform.position + Vector3.up * 2, 0.5f);
    }
}


