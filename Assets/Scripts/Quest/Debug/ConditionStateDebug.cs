using UnityEngine;

public class ConditionStateDebug : MonoBehaviour
{
    [SerializeField] private string debugName = "ConditionDebug";
    private MeshRenderer debugRenderer;
    private int debugCount = 0;

    private void Start()
    {
        debugRenderer = GetComponent<MeshRenderer>();
    }
    public void SetDebugState(bool state)
    {
        Debug.Log(debugName + " = " +  state +"(" + debugCount + ")");

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
