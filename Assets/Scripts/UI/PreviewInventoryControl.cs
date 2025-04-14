using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewInventoryControl : MonoBehaviour
{
    [Header("Preview Object")]
    [SerializeField] private float speed = 30f;
    [SerializeField] private Transform previewPosition;
    public Transform PreviewPosition
    {
        get => previewPosition;
    }
    
    [Header("Preview Camera")]
    public Transform camPosition;
    public float offset = 2f;
    public Vector3 direction = Vector3.back;
    
    void Update()
    { 
        previewPosition.transform.Rotate(Vector3.up, speed * Time.unscaledDeltaTime);
    }

    public void SetOffsetCamera(GameObject previewObject)
    {
        if (previewObject is null)
        {
            return;
        }
        
        // Récupère le renderer pour connaître la taille
        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("Renderer non trouvé sur l’enfant !");
            return;
        }

        float halfWidth = renderer.bounds.size.x / 2f;

        // Calculer la position finale de la caméra
        Vector3 finalOffset = direction.normalized * (offset + halfWidth);
        camPosition.transform.position = previewPosition.position + finalOffset;
    }
}
