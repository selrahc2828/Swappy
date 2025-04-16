using UnityEditor.UIElements;
using UnityEngine;

public class ObjectCondition : Condition
{
    private enum ValidationTypes
    {
        Tag,
        Layer
    }

    [Space(16)]
    public GameObject targetObject;
    [SerializeField] private ValidationTypes validationType;

    [Space(16)]
    [SerializeField] private string targetTag;
    [SerializeField] private LayerMask targetLayer;

    private void Start()
    {
        if (targetObject == null && attachedQuest != null)
        {
            Debug.LogError("Il n'y a pas de Target Object référencé alors que cette condition n'est pas additionelle");
        }
    }

    private void FixedUpdate()
    {
        if (targetObject != null && attachedQuest != null)
        {
            attachedQuest.SetCondition(this, CheckObjectParameters(targetObject));
        }
    }

    public override bool CheckObjectParameters(GameObject target)
    {
        switch (validationType)
        {
            case ValidationTypes.Tag:
                if (targetObject.CompareTag(targetTag))
                {
                    return validationBool;
                }
                break;

            case ValidationTypes.Layer:
                if (targetObject.layer == targetLayer)
                {
                    return validationBool;
                }
                break;
        }
        return !validationBool;
    }
}
