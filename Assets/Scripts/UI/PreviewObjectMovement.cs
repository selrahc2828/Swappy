using UnityEngine;
using UnityEngine.Serialization;

public class PreviewObjectMovement: MonoBehaviour
{
    [SerializeField] private float speed = 30f;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    void Update()
    { 
        transform.Rotate(Vector3.up, speed * Time.unscaledDeltaTime);
    }
}
