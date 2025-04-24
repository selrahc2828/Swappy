using UnityEngine;

public class AdditionalCondition : Condition
{
    public virtual bool CheckObjectParameters(GameObject target)
    {
        Debug.LogWarning("une version Override de CheckObjectParameters() n'existe pas !");
        return false;
    }
}
