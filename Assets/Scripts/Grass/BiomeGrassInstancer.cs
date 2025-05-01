using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class BiomeGrassInstancer : MonoBehaviour
{
    [Header("References")]
    public MeshFilter groundMeshFilter;
    public Mesh grassMesh;
    public Material grassMaterial;

    [Header("Grass Settings")]
    public int instanceCount = 10000;
    public float visibleDistance = 100f;
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public float uniformScaleMultiplier = 1f;

    [Header("Debug")]
    public bool showPositionGizmos = true;
    public Color positionGizmoColor = Color.green;
    public float gizmoSphereSize = 0.05f;

    public bool showMeshGizmos = false;
    public Color meshOutlineColor = new Color(0f, 1f, 0f, 0.1f);

    public bool showCullingRange = true;
    public Color cullingRangeColor = new Color(1f, 0.5f, 0f, 0.25f);

    private List<Matrix4x4> allMatrices = new List<Matrix4x4>();
    private List<Matrix4x4> visibleMatrices = new List<Matrix4x4>();
    private Transform cam;

    void Start()
    {
        cam = Camera.main?.transform;
        GenerateGrass();
    }

    public void GenerateGrass()
    {
        allMatrices.Clear();

        if (groundMeshFilter == null || groundMeshFilter.sharedMesh == null)
        {
            Debug.LogError("Ground MeshFilter or its Mesh is not assigned!");
            return;
        }

        Mesh mesh = groundMeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        Transform groundTransform = groundMeshFilter.transform;

        for (int i = 0; i < instanceCount; i++)
        {
            int triIndex = Random.Range(0, triangles.Length / 3) * 3;

            Vector3 v0 = vertices[triangles[triIndex + 0]];
            Vector3 v1 = vertices[triangles[triIndex + 1]];
            Vector3 v2 = vertices[triangles[triIndex + 2]];

            Vector3 localPoint = GetRandomPointInTriangle(v0, v1, v2);
            Vector3 worldPoint = groundTransform.TransformPoint(localPoint);

            Vector3 n0 = normals[triangles[triIndex + 0]];
            Vector3 n1 = normals[triangles[triIndex + 1]];
            Vector3 n2 = normals[triangles[triIndex + 2]];
            Vector3 localNormal = (n0 + n1 + n2).normalized;
            Vector3 worldNormal = groundTransform.TransformDirection(localNormal);

            Quaternion rot = Quaternion.LookRotation(Vector3.Cross(worldNormal, Vector3.right), worldNormal);
            float randomScale = Random.Range(scaleRange.x, scaleRange.y) * uniformScaleMultiplier;
            Vector3 scale = Vector3.one * randomScale;

            Matrix4x4 matrix = Matrix4x4.TRS(worldPoint, rot, scale);
            allMatrices.Add(matrix);
        }

        Debug.Log($"Placed {allMatrices.Count} grass instances.");
    }

    void Update()
    {
        if (cam == null) cam = Camera.main?.transform;
        if (cam == null) return;

        visibleMatrices.Clear();
        Vector3 camPos = cam.position;

        foreach (var matrix in allMatrices)
        {
            Vector3 pos = matrix.GetColumn(3);
            if ((pos - camPos).sqrMagnitude < visibleDistance * visibleDistance)
            {
                visibleMatrices.Add(matrix);
            }
        }

        const int batchSize = 1023;
        for (int i = 0; i < visibleMatrices.Count; i += batchSize)
        {
            int size = Mathf.Min(batchSize, visibleMatrices.Count - i);
            Graphics.DrawMeshInstanced(grassMesh, 0, grassMaterial, visibleMatrices.GetRange(i, size));
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (cam == null) return;

        if (showCullingRange)
        {
            Gizmos.color = cullingRangeColor;
            Gizmos.DrawWireSphere(cam.position, visibleDistance);
        }

        if (allMatrices == null) return;

        // Sphere Gizmos at position
        if (showPositionGizmos)
        {
            Gizmos.color = positionGizmoColor;
            foreach (var matrix in allMatrices)
            {
                Gizmos.DrawSphere(matrix.GetColumn(3), gizmoSphereSize);
            }
        }

        // Mesh outline
        if (showMeshGizmos && grassMesh != null)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            Gizmos.color = meshOutlineColor;

            foreach (var matrix in allMatrices)
            {
                Graphics.DrawMeshNow(grassMesh, matrix);
            }
        }
    }

    Vector3 GetRandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        float r1 = Random.value;
        float r2 = Random.value;
        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }
        return a + r1 * (b - a) + r2 * (c - a);
    }
}
