using UnityEngine;
using UnityEngine.EventSystems;

public class EventCondition : Condition
{
    [Space(16)]
    [Tooltip("Mettre à 0 pour désactiver le timer et rendre l'EventCondition true de manière permanente")]
    [SerializeField] private float permissivityTimer;
    [Tooltip("Uniquement utile pour donner une destination aux lignes de quêtes debug")]
    [SerializeField] private GameObject eventSource;

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
    protected override Vector3 GetQuestLineStart()
    {
        return eventSource.transform.position;
    }
}
