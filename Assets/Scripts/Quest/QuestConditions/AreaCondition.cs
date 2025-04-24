using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : Condition
{
    private enum ValidationTypes
    {
        OneOf,
        Precise,
        MinMax
    }

    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private AdditionalCondition additionalCondition;

    [SerializeField] private float preciseObjectNumber;
    [SerializeField] private float minObjectNumber;
    [SerializeField] private float maxObjectNumber;

    private List<GameObject> objectsInArera = new List<GameObject>();
    private Collider areaTrigger;

    private void Start()
    {
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("Il n'y as pas de Collider attach� � l'objet"); 
        }

        else
        {
            areaTrigger = GetComponent<Collider>();

            if (areaTrigger.isTrigger == false)
            {
                Debug.LogError("Le Collider attach� � l'objet n'est pas marqu� comme trigger");
            }
        }

        if (additionalCondition == null && validationType != ValidationTypes.OneOf)
        {
            Debug.LogError("Si validationType est dans un autre mode que OneOf, une condition additionelle doit �tre renseign�e pour v�rifier un param�tre d'objet plut�t que l'identit� d'un seul objet");
            Debug.Log("(C'est comme si vous demandiez � la police de retrouver deux 'Xavier Dupont de Ligon�s' plut�t qu'un seul en disant qu'ils sont plusieurs � avoir transorm� leur famille en terasse, ca peux pas fonctionner mdr.)");
        }

        if (additionalCondition == null && targetObject == null)
        {
            Debug.LogError("Il faut renseigner un targetObject ou alors une additionalCondition dans ce script pour qu'il y est un sens de l'utiliser.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsInArera.Add(other.gameObject);
        VerifyAreaCondition();
    }

    private void OnTriggerStay(Collider other)
    {
        if (additionalCondition != null)
        {
            VerifyAreaCondition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInArera.Remove(other.gameObject);
        VerifyAreaCondition();
    }

    private void VerifyAreaCondition()
    {
        if (targetObject != null && validationType == ValidationTypes.OneOf) 
        {
            foreach (GameObject target in objectsInArera)
            {
                if (targetObject == target)
                {
                    SetConditionState(true);
                    return;
                }
            }
            SetConditionState(false);
        }

        else if (additionalCondition != null)
        {
            float count = 0;

            switch (validationType)
            {
                case ValidationTypes.OneOf:
                    foreach (GameObject target in objectsInArera)
                    {
                        SetConditionState(additionalCondition.CheckObjectParameters(target));                           
                        return;
                    }
                    SetConditionState(false);
                    return;

                case ValidationTypes.Precise:
                    foreach (GameObject target in objectsInArera)
                    {
                        if (additionalCondition.CheckObjectParameters(target))
                        {
                            count++;
                        }
                    }
                    if (count == preciseObjectNumber)
                    {
                        SetConditionState(true);
                        return;
                    }
                    SetConditionState(false);
                    return;

                 case ValidationTypes.MinMax:
                    foreach (GameObject target in objectsInArera)
                    {
                        if (additionalCondition.CheckObjectParameters(target))
                        {
                            count++;
                        }
                    }
                    if (count >= minObjectNumber && count <= maxObjectNumber)
                    {
                        SetConditionState(true);
                        return;
                    }
                    SetConditionState(false);
                    return;

            }
        }
    }
}
