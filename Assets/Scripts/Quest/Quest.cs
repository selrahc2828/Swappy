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

    private Dictionary<QuestCondition, bool> ActiveConditions = new Dictionary<QuestCondition, bool>();


    public void ReferenceThisCondition(QuestCondition conditionScript, bool state) //Référence toutes les conditions d'une quête à l'initialisation dans un dictionnaire
    {
        ActiveConditions.Add(conditionScript, state);
    }

    public void ChangeQuestConditions(QuestCondition condition, bool state) //Change le state d'une des condition de la quête
    {
        ActiveConditions[condition] = state;
        CheckQuestConditions();
    }

    private void CheckQuestConditions() //Vérifie si la quête est accomplie en parcourant toutes les entrées du dictionnaires et en regardant si l'une d'elle est false
    {
        foreach (KeyValuePair<QuestCondition, bool> conditionRef in ActiveConditions)
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


