using UnityEngine;

public class ObjectCondition : AdditionalCondition
{
    private enum ValidationTypes
    {
        Tag,
        Layer
    }

    public GameObject targetObject;
    [SerializeField] private ValidationTypes validationType;

    [Space(8)]
    [SerializeField] private string targetTag;
    [SerializeField] private LayerMask targetLayer;


    private void FixedUpdate()
    {
        if (targetObject != null)
        {
            SetConditionState(CheckObjectParameters(targetObject));
        }
    }

    public override bool CheckObjectParameters(GameObject target)
    {
        switch (validationType)
        {
            case ValidationTypes.Tag:
                if (target.CompareTag(targetTag))
                {
                    return true;
                }
                break;

            case ValidationTypes.Layer:
                if (target.layer == targetLayer)
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
