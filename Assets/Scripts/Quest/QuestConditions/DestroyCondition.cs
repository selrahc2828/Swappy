using UnityEngine;

public class DestroyCondition : Condition
{
    private void OnDestroy()
    {
        attachedQuest.SetCondition(this, validationBool);
        attachedQuest.UnreferenceCondition(this);
    }
}
