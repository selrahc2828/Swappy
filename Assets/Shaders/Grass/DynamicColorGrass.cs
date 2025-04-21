using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicColorGrass : MonoBehaviour
{
    public float raycastHeightOffset = 1.0f;
    public LayerMask groundLayer;
    private Renderer rend;
    private MaterialPropertyBlock propBlock;
    public Texture2D textureSol;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("TerrainGrass"));
        propBlock = new MaterialPropertyBlock();
        UpdateGrassColor();
    }
    
    void UpdateGrassColor()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f, groundLayer))
        {
            Renderer groundRenderer = hit.collider.GetComponent<Renderer>();
            if (groundRenderer != null && groundRenderer.material.mainTexture is Texture2D tex)
            {
                Vector2 uv;
                if (TryGetUV(hit, out uv))
                {
                    // Color pixelColor = tex.GetPixelBilinear(uv.x, uv.y);
                    // ApplyColorToShader(pixelColor);
                    Vector2 pixelUV;
                
                    // Coordonnées UV
                    uv = hit.textureCoord;

                    // Conversion UV -> pixel
                    pixelUV.x = uv.x * textureSol.width;
                    pixelUV.y = uv.y * textureSol.height;

                    // Lire la couleur
                    Color pixelColor = textureSol.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                    ApplyColorToShader(pixelColor);
                }
            }
        }
    }

    void ApplyColorToShader(Color color)    
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_GroundColor", color);
        Debug.Log(color);
        rend.SetPropertyBlock(propBlock);
    }

    bool TryGetUV(RaycastHit hit, out Vector2 uv)
    {
        uv = hit.textureCoord;
        Debug.Log(hit.textureCoord);
        return true;
    }
}
