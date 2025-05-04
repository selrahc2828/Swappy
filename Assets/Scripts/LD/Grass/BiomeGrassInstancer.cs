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
    public Transform planetCenter;

    [Header("Grass Settings")]
    public int instanceCount = 10000;
    public float visibleDistance = 100f;
    [Range(0f, 180f)]
    public float cullingConeAngle = 120f;
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public float uniformScaleMultiplier = 1f;
    public float topFaceAngleThreshold = 15f; // degrés

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

    [ContextMenu("Regenerate Grass")]
    public void GenerateGrass()
    {
        allMatrices.Clear();

        if (groundMeshFilter == null || groundMeshFilter.sharedMesh == null || planetCenter == null)
        {
            Debug.LogError("Missing reference(s) for BiomeGrassInstancer.");
            return;
        }

        Mesh mesh = groundMeshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        Transform groundTransform = groundMeshFilter.transform;

        int placed = 0;
        int attempts = 0;
        while (placed < instanceCount && attempts < instanceCount * 10)
        {
            attempts++;
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
            Vector3 worldNormal = groundTransform.TransformDirection(localNormal).normalized;

            // Check if face is "upward"
            Vector3 toCenter = (worldPoint - planetCenter.position).normalized;
            float angle = Vector3.Angle(worldNormal, toCenter);
            if (angle > topFaceAngleThreshold) continue;

            // Random rotation around local Y (normal)
            Quaternion alignToNormal = Quaternion.LookRotation(Vector3.Cross(worldNormal, Vector3.right), worldNormal);
            float randomYRot = Random.Range(0f, 360f);
            Quaternion randomY = Quaternion.AngleAxis(randomYRot, Vector3.up);
            Quaternion finalRot = alignToNormal * randomY;

            float randomScale = Random.Range(scaleRange.x, scaleRange.y) * uniformScaleMultiplier;
            Vector3 scale = Vector3.one * randomScale;

            Matrix4x4 matrix = Matrix4x4.TRS(worldPoint, finalRot, scale);
            allMatrices.Add(matrix);
            placed++;
        }

        Debug.Log($"Placed {allMatrices.Count} grass instances (attempts: {attempts}).");
    }

    void Update()
    {
        if (cam == null) cam = Camera.main?.transform;
        if (cam == null) return;

        visibleMatrices.Clear();
        Vector3 camPos = cam.position;
        Vector3 camForward = cam.forward;
        float cosAngleThreshold = Mathf.Cos(cullingConeAngle * Mathf.Deg2Rad * 0.5f);

        foreach (var matrix in allMatrices)
        {
            Vector3 pos = matrix.GetColumn(3);
            Vector3 toGrass = pos - camPos;
            float distanceSqr = toGrass.sqrMagnitude;

            if (distanceSqr < visibleDistance * visibleDistance)
            {
                Vector3 dir = toGrass.normalized;
                float cosAngle = Vector3.Dot(camForward, dir);
                if (cosAngle >= cosAngleThreshold)
                {
                    visibleMatrices.Add(matrix);
                }
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
            Quaternion rotation = Quaternion.LookRotation(cam.forward);
            Matrix4x4 coneMatrix = Matrix4x4.TRS(cam.position, rotation, Vector3.one);
            Gizmos.matrix = coneMatrix;
            Gizmos.DrawFrustum(Vector3.zero, cullingConeAngle, visibleDistance, 0.1f, 1f);
            Gizmos.matrix = Matrix4x4.identity;
        }

        if (allMatrices == null) return;

        if (showPositionGizmos)
        {
            Gizmos.color = positionGizmoColor;
            foreach (var matrix in allMatrices)
            {
                Gizmos.DrawSphere(matrix.GetColumn(3), gizmoSphereSize);
            }
        }

        if (showMeshGizmos && grassMesh != null)
        {
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

