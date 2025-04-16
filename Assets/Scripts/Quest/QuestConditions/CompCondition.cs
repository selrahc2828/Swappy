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
        None,
        Solo,
        Duo,
        Fusion,
        StrictFusion
    }

    [Space(16)]
    [Tooltip("Si targetObject vide, le script checkera à chaque vol / réattribution de comportement")]
    public GameObject targetObject;
    [SerializeField] private ValidationTypes validationType;

    [Space(16)]
    [SerializeField] private FirstState targetComportment;
    [SerializeField] private CompCategory targetCompCategory;

    private void Update()
    {
        //A remplacer par une activation lorsque le joueur input un vol / réattribution de comportement
        if (targetObject != null)
        {
            attachedQuest.SetCondition(this, CheckObjectParameters(targetObject));
        }
    }

    public override bool CheckObjectParameters(GameObject target)
    {
        if (target.GetComponent<ComportementsStateMachine>() == null)
        {
            if (attachedQuest != null)
            {
                Debug.LogError("Il n'y a pas de ComportementsStateMachine sur l'objet target par CompCondition !");
            }
            return !validationBool;
        }
        ComportementState compState = (ComportementState)target.GetComponent<ComportementsStateMachine>().currentState;
      
        switch (validationType)
        {
            case ValidationTypes.Comportment:

                if (compState.stateValue == (int)targetComportment)
                {
                    return(validationBool);
                }
                return (!validationBool);

            case ValidationTypes.CompCategory:

                int leftComp = compState.leftValue;
                int rightComp = compState.rightValue;

                switch (targetCompCategory)
                {
                    case CompCategory.None:
                        if (rightComp == 0 && leftComp == 0)
                        {
                            return (validationBool);
                        }
                        break;

                    case CompCategory.Solo:
                        if (rightComp == 0 && leftComp != 0)
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

    protected override Vector3 GetQuestLineStart()
    {
        return base.GetQuestLineStart();
    }
}
