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
        StrictFusion
    }

    [Space(16)]
    [Tooltip("Si targetObject vide, le script checkera à chaque vol / réattribution de comportement")]
    public GameObject targetObject;
    [SerializeField] private bool validationBool = true;
    [SerializeField] private ValidationTypes validationType;

    [Space(16)]
    [SerializeField] private FirstState targetComportment;
    [SerializeField] private CompCategory targetCompCategory;

    private void Update()
    {
        //A remplacer par une activation lorsque le joueur input un vol / réattribution de comportement
        if (targetObject != null)
        {
            CheckObjectParameters(targetObject);
        }
    }

    protected override bool CheckObjectParameters(GameObject target)
    {
        if (target.GetComponent<ComportementState>() == null)
        {
            Debug.LogError("Il n'y a pas de ComportementState sur l'objet target par CompCondition !");
            return !validationBool;
        }

        switch (validationType)
        {
            case ValidationTypes.Comportment:

                if (target.GetComponent<ComportementState>().stateValue == (int)targetComportment)
                {
                    return(validationBool);
                }
                return (!validationBool);

            case ValidationTypes.CompCategory:

                int rightComp = target.GetComponent<ComportementState>().rightValue;
                int leftComp = target.GetComponent<ComportementState>().leftValue;

                switch (targetCompCategory)
                {
                    case CompCategory.Solo:
                        if (rightComp == 0)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.Duo:
                        if (rightComp != 0 && rightComp == leftComp)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.Fusion:
                        if (rightComp != 0)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.StrictFusion:
                        if (rightComp != 0 && rightComp != leftComp)
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
