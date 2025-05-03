using UnityEngine;

public class VelocityCondition : AdditionalCondition
{
    private enum VelocityType
    {
        VelocityMagnitude,
        AngularVelocity,
    }

    [Space(16)]
    public GameObject targetObject;
    [SerializeField] private VelocityType validationType;
    [SerializeField] private float targetVelocityMin;
    [SerializeField] private float targetVelocityMax;

    private void FixedUpdate()
    {
        if (targetObject != null)
        {
            SetConditionState(CheckObjectParameters(targetObject));
        }
    }

    public override bool CheckObjectParameters(GameObject target)
    {
        Rigidbody targetRb = targetObject.GetComponent<Rigidbody>();
        float velocity = targetRb.velocity.magnitude;
        float angular = targetRb.angularVelocity.magnitude;

        switch (validationType)
        {
            case VelocityType.VelocityMagnitude:
                if (velocity >= targetVelocityMin && velocity <= targetVelocityMax)
                {
                    return true;
                }
                break;

            case VelocityType.AngularVelocity:
                if (angular >= targetVelocityMin && angular <= targetVelocityMax)
                {
                    return true;
                }
                break;
        }
        return false;
    }
}
