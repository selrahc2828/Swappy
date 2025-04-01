using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<bool> Conditions = new List<bool>();
}
