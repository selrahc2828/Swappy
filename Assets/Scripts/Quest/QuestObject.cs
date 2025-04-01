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

    private void Start()
    {

    }
}
