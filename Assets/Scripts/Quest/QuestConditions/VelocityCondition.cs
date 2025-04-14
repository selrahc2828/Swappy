using UnityEngine;

public class VelocityCondition : Condition
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


    private void Start()
    {
        if (targetObject == null && attachedQuest != null)
        {
            Debug.LogError("Il n'y a pas de Target Object référencé alors que cette condition n'est pas additionelle");
        }
    }

    protected override bool CheckObjectParameters(GameObject target)
    {
        Rigidbody targetRb = targetObject.GetComponent<Rigidbody>();
        float velocity = targetRb.velocity.magnitude;
        float angular = targetRb.angularVelocity.magnitude;

        switch (validationType)
        {
            case VelocityType.VelocityMagnitude:
                if (velocity >= targetVelocityMin && velocity <= targetVelocityMax)
                {
                    return validationBool;
                }
                break;

            case VelocityType.AngularVelocity:
                if (angular >= targetVelocityMin && angular <= targetVelocityMax)
                {
                    return validationBool;
                }
                break;
        }
        return !validationBool;
    }
}
