using UnityEngine;
using UnityEngine.EventSystems;

public class EventCondition : QuestCondition
{
    private enum ValidationTypes
    {
        [Tooltip("Pour li� l'accomplissement qu�te � une condition d'une autre Qu�te")]
        OnQuestFinished,
        [Tooltip("Lorsque l'objet sp�cifi� est d�truit")]
        OnDestroy,
        [Tooltip("Autre Events, qui peuvent acc�der directement � la fonction SetEventConditonState depuis un autre script")]
        OtherEvent
    }

    [Space(16)]
    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private Quest questScript;
    [SerializeField] private GameObject onDestroyedObject;

    [Space(16)]
    [Tooltip("Mettre � 0 pour d�sactiver le timer et rendre l'EventCondition true de mani�re permanente")]
    [SerializeField] private float permissivityTimer;

    private bool isConditionTrue;
    private float timer;

    public void SetEventConditionState(bool state)
    {
        SetConditionState(state);

        isConditionTrue = state;
        timer = permissivityTimer;
    }

    private void Update()
    {
        if (permissivityTimer != 0 && isConditionTrue)
        {
            timer -= Time.deltaTime;
            Debug.Log("Timer EventCondition: " + timer);

            if (timer <= 0)
            {
                SetConditionState(false);

                isConditionTrue = false;
            }

        }
    }
}
