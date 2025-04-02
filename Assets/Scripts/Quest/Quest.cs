using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QuestType
{
    NPCQuest,
    LoreQuest,
    Collectible,
    BlankActivity
}

public class Quest : MonoBehaviour
{
    public string QuestName;
    public QuestType QuestType;
    public UnityEvent QuestEvent;

    private Dictionary<QuestCondition, bool> ActiveConditions = new Dictionary<QuestCondition, bool>();

    public void ReferenceThisCondition(QuestCondition conditionScript, bool state) //Référence toutes les conditions d'une quête à l'initialisation dans un dictionnaire
    {
        ActiveConditions.Add(conditionScript, state);
        Debug.Log("Number of Conditions: " +  ActiveConditions.Count);
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
                Debug.Log("Quest '" + QuestName + "' is false");
                return;  
            }
        }

        Debug.Log("Quest '" + QuestName + "' accomplished");
        QuestEvent.Invoke();
    }
}


