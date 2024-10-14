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

    void Start()
    {
        StealsToApply = new Dictionary<MonoBehaviour, System.Type>();
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {

        while (true)
        {
            yield return new WaitForSeconds(SpawnRate);

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
}
