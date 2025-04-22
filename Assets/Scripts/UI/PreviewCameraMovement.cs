using UnityEngine;

public class PreviewCameraMovement : MonoBehaviour
{
    public Transform pivot; 
    public float orbitSpeed = 30f; 

    void Update()
    {
        if (pivot != null)
        {
            transform.RotateAround(pivot.position, Vector3.up, orbitSpeed * Time.unscaledDeltaTime);
        }
    }
}
