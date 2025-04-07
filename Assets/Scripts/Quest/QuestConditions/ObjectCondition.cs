using UnityEngine;

public class ObjectCondition : Condition
{
    private enum ValidationTypes
    {
        Tag,
        Layer,
        Comportment,
        CompCategory,
        Velocity,
        AngularVelocity,
    }
    private enum OperatorTypes
    {
        Equal,
        Min
    }
    private enum CompCategory
    {
        Solo,
        Duo,
        Fusion,
        StrictFusion,
    }

    [Space(16)]
    public GameObject targetObject;
    [SerializeField] private bool validationBool = true;
    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private OperatorTypes operatorType;

    [Space(16)]
    [SerializeField] private string targetTag;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private FirstState targetComportment;
    [SerializeField] private float targetVelocity;
    [SerializeField] private float targetAngularVelocity;

    private GameManager gameManager;
    private GameObject player;

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.player;
    }

    private void FixedUpdate()
    {
        if (targetObject != null)
        {
            if (validationType == ValidationTypes.Velocity || validationType == ValidationTypes.AngularVelocity)
            {
                attachedQuest.SetCondition(this, CheckObjectParameters(targetObject));
            }
        }
    }

    /*public bool CheckObjectParameters(GameObject target)
    {
        switch (validationType)
        {
            case ValidationTypes.Tag:
                if (targer)
                break;
            case ValidationTypes.Comportment:

                if (target.GetComponent<ComportementState>().stateValue == (int)targetComportment)
                {
                    SetConditionState(validationBool);
                }
                break;

            case ValidationTypes.Velocity:
                if (operatorType == OperatorTypes.Min)
                {
                    if (target.GetComponent<Rigidbody>().velocity.magnitude >= targetVelocity)
                    {
                        SetConditionState(validationBool);
                    }
                }
                else
                {
                    if (target.GetComponent<Rigidbody>().velocity.magnitude == targetVelocity)
                    {
                        SetConditionState(validationBool);
                    }
                }
                break;

            case ValidationTypes.AngularVelocity:
                if (operatorType == OperatorTypes.Min)
                {
                    if (target.GetComponent<Rigidbody>().angularVelocity.magnitude >= targetAngularVelocity)
                    {
                        SetConditionState(validationBool);
                    }
                }
                else
                {
                    if (target.GetComponent<Rigidbody>().angularVelocity.magnitude == targetAngularVelocity)
                    {
                        SetConditionState(validationBool);
                    }
                }
                break;

            default:
                break;
        }
        return false;
    }*/
}
