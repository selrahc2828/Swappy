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

public class QuestObject : MonoBehaviour
{
    public string QuestName;
    public QuestType QuestType;
    public UnityEvent QuestEvent;

    private List<bool> _conditions = new List<bool>();
    public List<bool> Conditions
    {
        private get { return _conditions; }
        set 
        {
            _conditions = value;
            CheckQuestConditions(); 
        }
        
    }
    public void CheckQuestConditions()
    {
        foreach (bool condition in Conditions)
        {
            if (condition == false)
            {
                Debug.Log("Quest '" + QuestName + "' is false");
                return;  
            }
        }

        Debug.Log("Quest '" + QuestName + "' accomplished");
        QuestEvent.Invoke();
    }
}


