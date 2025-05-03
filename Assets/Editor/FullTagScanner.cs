using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class FullTagScanner : EditorWindow
{
    private string tagToSearch = "Player";
    private Vector2 scrollPos;

    private List<string> matchingObjects = new List<string>();
    private int prefabCount = 0;
    private int sceneObjectCount = 0;

    [MenuItem("Tools/Scanner de tags complet")]
    public static void ShowWindow()
    {
        GetWindow<FullTagScanner>("Scanner de Tags");
    }

    void OnGUI()
    {
        tagToSearch = EditorGUILayout.TagField("Tag à rechercher", tagToSearch);

        if (GUILayout.Button("Analyser tout le projet"))
        {
            ScanProject();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Objets avec le tag dans les prefabs :", prefabCount.ToString());
        EditorGUILayout.LabelField("Objets avec le tag dans toutes les scènes :", sceneObjectCount.ToString());
        EditorGUILayout.LabelField("Total :", (prefabCount + sceneObjectCount).ToString());

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Objets trouvés :", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
        foreach (string entry in matchingObjects)
        {
            EditorGUILayout.LabelField(entry);
        }
        EditorGUILayout.EndScrollView();
    }

    void ScanProject()
    {
        prefabCount = 0;
        sceneObjectCount = 0;
        matchingObjects.Clear();

        // Analyse des prefabs
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (IsPathReadOnly(path)) continue;

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            GameObject[] children = prefab.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject).ToArray();
            foreach (GameObject go in children)
            {
                if (go.CompareTag(tagToSearch))
                {
                    prefabCount++;
                    matchingObjects.Add($"[Prefab] {path} > {go.name}");
                }
            }
        }

        // Analyse des scènes
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in sceneGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (IsPathReadOnly(path)) continue;

            Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            GameObject[] rootObjects = scene.GetRootGameObjects();

            foreach (GameObject root in rootObjects)
            {
                GameObject[] children = root.GetComponentsInChildren<Transform>(true).Select(t => t.gameObject).ToArray();
                foreach (GameObject go in children)
                {
                    if (go.CompareTag(tagToSearch))
                    {
                        sceneObjectCount++;
                        matchingObjects.Add($"[Scene] {path} > {go.name}");
                    }
                }
            }

            EditorSceneManager.CloseScene(scene, true);
        }
    }

    bool IsPathReadOnly(string path)
    {
        return path.StartsWith("Packages/") || path.StartsWith("Library/");
    }
}
