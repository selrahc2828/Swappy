using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : InteractionSystem
{
    public override string interactionText => "Pickup"; // "=>" simplification de : get { return "Grab"; }
    
    public ItemData itemData;
    private InventorySystem inventory;
    private TapeSystem tapeSystem;

    public override void Initialize()
    {
        if (itemData == null)
        {
            Debug.LogError("PickUpInteraction: itemData is null");
            return;
        }
        
#if !UNITY_EDITOR
        if (itemData is TapeData)
        {
            TapeData tapeData = itemData as TapeData;
            if (tapeData.isUnlocked == true)// s'il est déjà debloquer on ne le fait pas spawn
            {
                return;
            }
        }
#endif
        
        inventory = FindObjectOfType<InventorySystem>(); // voir pour ref ailleur
        tapeSystem = FindObjectOfType<TapeSystem>(); // voir pour ref ailleur
            
        Instantiate(itemData.itemPrefab, transform.position, Quaternion.identity, transform);
    }

    public override void Interact()
    {
        base.Interact();

        Debug.Log("PickUpInteraction Interact");

        PickUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        switch (itemData)
        {
            case CollectibleData collectibleData:
                if (inventory == null)
                    return;
                inventory.AddItem(collectibleData);
                break;
            case TapeData tapeData:
                tapeSystem.SetLockTape(tapeData, true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(itemData));
        }
        
        Destroy(gameObject); 
    }
    
    private void OnDrawGizmos() {
        // Vérifie que l'objet a un Collider
        Collider collider = GetComponent<Collider>();
        if (collider != null) {
            // Dessine un gizmo en fonction du type de collider
            Gizmos.color = Color.green;  // Choisir la couleur du gizmo

            // Si le collider est de type BoxCollider
            if (collider is BoxCollider) {
                BoxCollider box = (BoxCollider)collider;
                Gizmos.DrawWireCube(box.bounds.center, box.bounds.size);
            }
            // Si le collider est de type SphereCollider
            else if (collider is SphereCollider) {
                SphereCollider sphere = (SphereCollider)collider;
                Gizmos.DrawWireSphere(sphere.bounds.center, sphere.radius);
            }
            // Si le collider est de type CapsuleCollider
            else if (collider is CapsuleCollider) {
                CapsuleCollider capsule = (CapsuleCollider)collider;
                Gizmos.DrawWireSphere(capsule.bounds.center, capsule.radius);
            }
            // Si le collider est de type MeshCollider
            else if (collider is MeshCollider) {
                MeshCollider mesh = (MeshCollider)collider;
                Gizmos.DrawWireMesh(mesh.sharedMesh, mesh.transform.position, mesh.transform.rotation, mesh.transform.localScale);
            }
        }
    }
}
