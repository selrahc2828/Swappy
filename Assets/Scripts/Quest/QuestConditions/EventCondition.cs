using UnityEngine;
using UnityEngine.EventSystems;

public class EventCondition : QuestCondition
{
    private enum ValidationTypes
    {
        [Tooltip("Pour lié l'accomplissement quête à une condition d'une autre Quête")]
        OnQuestFinished,
        [Tooltip("Lorsque l'objet spécifié est détruit")]
        OnDestroy,
        [Tooltip("Autre Events, qui peuvent accéder directement à la fonction SetEventConditonState depuis un autre script")]
        OtherEvent
    }

    [Space(16)]
    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private Quest questScript;
    [SerializeField] private GameObject onDestroyedObject;

    [Space(16)]
    [Tooltip("Mettre à 0 pour désactiver le timer et rendre l'EventCondition true de manière permanente")]
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
