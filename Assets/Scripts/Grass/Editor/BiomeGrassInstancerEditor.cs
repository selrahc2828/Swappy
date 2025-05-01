using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BiomeGrassInstancer))]
public class BiomeGrassInstancerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BiomeGrassInstancer instancer = (BiomeGrassInstancer)target;

        if (Application.isPlaying)
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Regenerate Grass"))
            {
                instancer.GenerateGrass();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Enter Play Mode to use the Regenerate button.", MessageType.Info);
        }
    }
}