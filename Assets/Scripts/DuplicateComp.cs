using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateComp : Comportment
{

    [SerializeField]
    private bool DestroyOnDuplicate;

    [SerializeField]
    private bool InfiniteSpawn;

    [SerializeField]
    private int NbrOfDuplicate;

    [SerializeField]
    private bool DoesChildrenDuplicate;

    [SerializeField]
    private float TimeBeforeDuplication;
    [SerializeField]
    private float duplicatetimer;

    private void Start()
    {
        //vérifie que le timer est à 0 quand le script commence
        duplicatetimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //timer classique
        duplicatetimer += Time.deltaTime;


        if (duplicatetimer >= TimeBeforeDuplication)
        {
            //spawn le nombre d'objet décidé par "NbrOfDuplicate"
            for (int i = 0; i < NbrOfDuplicate; i++)
            {
                GameObject Spawned = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);

                //si DoesChildrenDuplicate est faux, supprime le script de l'objet après la duplication.
                if (DoesChildrenDuplicate == false)
                {
                    Destroy(Spawned.GetComponent<DuplicateComp>());
                }


            }

            if (DestroyOnDuplicate)
            {
                Destroy(this.gameObject);
            }

            if (!InfiniteSpawn)
            {
                Destroy(this.GetComponent<DuplicateComp>());
            }

            duplicatetimer = 0;
        }
    }
}
