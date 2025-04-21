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
    MultipleDiscrete,
    MultipleContinuous,
    CountDiscrete,
    CountContinuous,
}

public class Quest : MonoBehaviour
{
    public string QuestName;
    public QuestTypes QuestType;
    [SerializeField] private QuestRedundancy Redundancy;
    [SerializeField] private int validationCounter;
    public UnityEvent QuestEvent;

    public readonly Dictionary<QuestTypes, Color> QuestColors = new Dictionary<QuestTypes, Color>()
    {   {QuestTypes.NPCQuest, Color.red},
        {QuestTypes.LoreQuest, Color.blue},
        {QuestTypes.Collectible, Color.magenta},
        {QuestTypes.BlankActivity, Color.green} };

    private Dictionary<Condition, bool> ActiveConditions = new Dictionary<Condition, bool>();
    private bool hasBeenValidatedOnce;
    private int validatedCounter;


    public void ReferenceCondition(Condition conditionScript, bool state) //Référence toutes les conditions d'une quête à l'initialisation dans un dictionnaire
    {
        ActiveConditions.Add(conditionScript, state);
    }

    public void UnreferenceCondition(Condition conditionScript)
    {
        ActiveConditions.Remove(conditionScript);
    }

    public void SetCondition(Condition condition, bool state) //Change le state d'une des condition de la quête
    {
        switch (Redundancy)
        {
            case QuestRedundancy.OneOf:
                if (hasBeenValidatedOnce)
                {
                    return;
                }
                break;

            case QuestRedundancy.MultipleDiscrete:
                if (ActiveConditions[condition] == state)
                {
                    return;
                }
                break;

            case QuestRedundancy.CountDiscrete:
                if (ActiveConditions[condition] == state)
                {
                    return;
                }
                break;

            default:
                break;
        }

        ActiveConditions[condition] = state;
        CheckConditions();
    }

    private void CheckConditions() //Vérifie si la quête est accomplie en parcourant toutes les entrées du dictionnaire et en regardant si l'une d'elle est false
    {


        foreach (KeyValuePair<Condition, bool> conditionRef in ActiveConditions)
        {
            if (conditionRef.Value == false)
            {
                return;
            }
        }

        if (Redundancy == QuestRedundancy.CountContinuous || Redundancy == QuestRedundancy.CountDiscrete)
        {
            validatedCounter++;
            if (validatedCounter != validationCounter)
            {
                return;
            }
        }
        
        Debug.Log("Quest '" + QuestName + "' accomplished");
        hasBeenValidatedOnce = true;
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


