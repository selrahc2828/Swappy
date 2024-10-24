using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class C_Spawner : Comportment
{
    public GameObject objectSpawned;
    public float spawnTime;

    [SerializeField] private Transform spawnPoint;

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
            GameObject instantiatedObject = Instantiate(objectSpawned);

            if (spawnPoint)
            {
                instantiatedObject.transform.position = spawnPoint.transform.position;   
            }

            else
            {
                instantiatedObject.transform.position = transform.position + Vector3.one * 2;
            }

            ApplyComportmentsTo(instantiatedObject);
            spawnTimer = 0f;
        }
    }

    public void ReferenceComportments(Comportment NewComp)
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
