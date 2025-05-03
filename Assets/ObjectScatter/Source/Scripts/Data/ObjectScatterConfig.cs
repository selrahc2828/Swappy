using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Scatter Config", menuName = "ObjectScatter/Config")]
public class ObjectScatterConfig : ScriptableObject
{
    public Color inspectorFoldoutColor = Color.cyan;
    //public int itemsProcessedPerFrame = 40; (TODO)

#if UNITY_EDITOR
    public static ObjectScatterConfig GetInstance()
    {
        List<ObjectScatterConfig> objectsInScene = new List<ObjectScatterConfig>();

        foreach (ObjectScatterConfig go in Resources.FindObjectsOfTypeAll(typeof(ObjectScatterConfig)) as ObjectScatterConfig[])
        {
            objectsInScene.Add(go);
        }

        return objectsInScene.Count > 0 ? objectsInScene[0] : null;
    }
#endif 
}
