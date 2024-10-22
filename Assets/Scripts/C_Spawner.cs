using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class C_Spawner : Comportment
{
    [SerializeField] private GameObject objectSpawned;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnTime;

    private List<Component> listComportement = new List<Component>();
    private float spawnTimer;

    void Start()
    {
        ReferenceComportments(null); 
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime)
        {
            GameObject instantiatedObject = Instantiate(objectSpawned, transform.position, Quaternion.identity);
            ApplyComportmentsTo(instantiatedObject);

            spawnTimer = 0f;
        }
    }

    private void ReferenceComportments(Comportment NewComp)
    {
        if (NewComp) //Quand on appel la fonction en possédant déjà le nouveau comportement à référencer
        {
            NewComp.enabled = false;
            listComportement.Add(NewComp);
            Debug.Log("Add to list : " + NewComp);
        }

        else
        {
            foreach (Comportment Comportment in GetComponents<Comportment>()) //Quand on appel la fonction sans savoir quels sont les comportements à référencer (start)
            {
                if (Comportment != this && Comportment.isActiveAndEnabled)
                {
                    Comportment.enabled = false;
                    listComportement.Add(Comportment);
                    Debug.Log("Add to list : " + Comportment);
                }
            }
        }
    }

    private void ApplyComportmentsTo(GameObject objectTarget)
    {
        foreach (Comportment component in listComportement)
        {
            System.Type type = component.GetType();
            Component newComp = objectTarget.gameObject.AddComponent(type);

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                field.SetValue(newComp, field.GetValue(component));
            }
        }
    }

    private void OnDisable()
    {
        foreach (Comportment component in listComportement )
        {
            component.enabled = true;
        }
    }
}
