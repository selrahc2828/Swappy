using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BakedInstance
{
    [SerializeField]
    GameObject gameObject;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Material material;

    [SerializeField]
    List<CombineInstance> combineInstances = new List<CombineInstance>();

    public int Size => combineInstances.Count;

    public BakedInstance(Transform parent, Material mat, string appendName)
    {
        material = mat;

        gameObject = new GameObject("Baked_" + appendName);
        gameObject.transform.SetParent(parent);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = material;
    }

    public bool AddInstance(GameObject instance)
    {
        if (instance.TryGetComponent<MeshFilter>(out var mf))
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            ci.transform = Matrix4x4.TRS(instance.transform.localPosition, instance.transform.localRotation, instance.transform.localScale);

            combineInstances.Add(ci);

            return true;
        }

        return false;
    }

    public void Bake()
    {
        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.name = gameObject.name + "_mesh";
        meshFilter.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.sharedMesh.CombineMeshes(combineInstances.ToArray());
    }

    public void Reset()
    {
        if(combineInstances == null)
            combineInstances = new List<CombineInstance>();

        combineInstances.Clear();
        meshFilter.sharedMesh = new Mesh();
        meshFilter.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
    }

    public void Destroy()
    {
        if (!Application.isPlaying)
        {
            GameObject.DestroyImmediate(gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }

    }
}
