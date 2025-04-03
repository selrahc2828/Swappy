using System.Collections;
using UnityEngine;

public class CompCondition: Condition
{
    private enum ValidationTypes
    {
        Comportment,
        CompCategory
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

    [Space(16)]
    [SerializeField] private FirstState targetComportment;
    [SerializeField] private CompCategory targetCompCategory;

    protected override bool CheckObjectParameters(GameObject target)
    {
        switch (validationType)
        {
            case ValidationTypes.Comportment:

                if (target.GetComponent<ComportementState>().stateValue == (int)targetComportment)
                {
                    return(validationBool);
                }
                return (!validationBool);

            case ValidationTypes.CompCategory:
                switch (targetCompCategory)
                { 
                    case CompCategory.Solo:
                        if (target.GetComponent<ComportementState>().rightValue == 0)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.Duo:
                        if (target.GetComponent<ComportementState>().leftValue == target.GetComponent<ComportementState>().rightValue)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.Fusion:
                        if (target.GetComponent<ComportementState>().leftValue == 0)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.StrictFusion:
                        if (target.GetComponent<ComportementState>().leftValue == 0)
                        {
                            return (validationBool);
                        }
                        break;
                
                    default:
                        return(!validationBool);

                }
                break;
            default:
                return (!validationBool);
        }
        return(!validationBool);
    }
}
