using UnityEngine;

public class CollideCondition : Condition
{
    private enum ValidationTypes
    {
        None,
        ObjectSpecific,
        VelocitySpecific,
        BothSpecific
    }

    [Space(16)]
    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private AdditionalCondition additionalCondition;
    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;

    private void Start()
    {
        if (validationType == ValidationTypes.ObjectSpecific || validationType == ValidationTypes.BothSpecific)
        {
            if (additionalCondition == null)
            {
                Debug.LogError("Il n'y a pas de référence à une additionalCondition alors que le script en a besoin pour fonctionner en mode ObjectSpecififc ou BothSpecific !");
            }
        }

        else
        {
            if (additionalCondition != null)
            {
                Debug.LogWarning("Il n'y a pas besoin de de référence à une additionalCondition dans ce script quand il est en mode None ou VelocitySpecific");   
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float colMagnitude = 0;
        float thisMagnitude = 0;

        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            colMagnitude = collision.rigidbody.velocity.magnitude;
        }
        if (this.gameObject.GetComponent<Rigidbody>())
        {
            thisMagnitude = this.GetComponent<Rigidbody>().velocity.magnitude;
        }

        switch (validationType)
        {
            case ValidationTypes.None:
                SetConditionState(true);
                break;

            case ValidationTypes.ObjectSpecific:
                SetConditionState(additionalCondition.CheckObjectParameters(collision.gameObject));
                break;

            case ValidationTypes.VelocitySpecific:
                if (CheckVelocityMinMax(colMagnitude, thisMagnitude))
                {
                    SetConditionState(true);
                }
                SetConditionState(false);
                break;

            case ValidationTypes.BothSpecific:
                if (CheckVelocityMinMax(colMagnitude, thisMagnitude))
                {
                    SetConditionState(additionalCondition.CheckObjectParameters(collision.gameObject));
                }
                SetConditionState(false);
                break;
        }
    }

    private bool CheckVelocityMinMax(float velocity1, float velocity2)
    {
        if (velocity1 >= minVelocity && velocity2 <= maxVelocity)
        {
            return true;
        }
        else if (velocity1 >= minVelocity && velocity2 <= maxVelocity)
        {
            return true;
        }
        return false;
    }
}
