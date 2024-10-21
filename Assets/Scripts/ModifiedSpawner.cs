using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ModifiedSpawner : MonoBehaviour
{
    public float SpawnRate;
    public Transform spwanPoint;
    public GameObject prefab;
    public Dictionary<MonoBehaviour, System.Type> StealsToApply;

    private float timer;

    void Start()
    {
        StealsToApply = new Dictionary<MonoBehaviour, System.Type>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnRate)
        {
            SpawnObject();
            timer = 0;
        }
    }

    private void SpawnObject()
    {
        if (prefab)
        {
            GameObject InstantiatedObject = Instantiate(prefab, spwanPoint.position, Quaternion.identity);

            if (StealsToApply != null)
            {
                foreach (KeyValuePair<MonoBehaviour, System.Type> script in StealsToApply)
                {
                    Component newComponent = InstantiatedObject.AddComponent(script.Value);

                    foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        field.SetValue(newComponent, field.GetValue(script.Key));

                    }
                }
            }
            InstantiatedObject = null;
        }
    }
}

