using UnityEngine;

public class OperatorCondition : Condition
{
    public virtual void OperatorInput(bool state, Condition source)
    {
        Debug.LogError("une version Override de OperatorInput() n'existe pas !");
        return;
    }
}
