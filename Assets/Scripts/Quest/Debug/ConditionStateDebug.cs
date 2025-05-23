using UnityEngine;

public class ConditionStateDebug : MonoBehaviour
{
    [SerializeField] private string debugName = "ConditionDebug";
    private MeshRenderer debugRenderer;
    private int debugCount = 0;
    public void SetDebugState(bool state)
    {
        Debug.Log(debugName + " state = " +  state +"(" + debugCount + ")");

        if (state)
        {
            debugRenderer.material.color = Color.green;
        }
        else
        {

            debugRenderer.material.color = Color.red;

        }
    }
}
