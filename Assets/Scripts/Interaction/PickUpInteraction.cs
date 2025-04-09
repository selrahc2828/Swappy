using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : InteractionSystem
{
    public override string interactionText => "Pickup"; // "=>" simplification de : get { return "Grab"; }
    
    public ItemData itemData;

    public override void Initialize()
    {
        if (itemData == null)
        {
            Debug.LogError("PickUpInteraction: itemData is null");
            return;
        }
        Instantiate(itemData.itemPrefab, transform.position, Quaternion.identity, transform);
    }

    public override void Interact()
    {
        base.Interact();

        Debug.Log("PickUpInteraction Interact");

        InventorySystem inventory = GameManager.Instance?.player.gameObject.GetComponent<InventorySystem>();
        if (inventory == null)
            return;
        
        inventory.AddItem(itemData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<InventorySystem>().AddItem(itemData);
            Destroy(gameObject);
        }
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
