using UnityEngine;
using UnityEngine.EventSystems;

public class EventCondition : QuestCondition
{
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
