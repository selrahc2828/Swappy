using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [Header("Cible")]
    public Transform target;

    [Header("Mouvement")]
    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.1f;

    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // on reste à plat

        if (direction.magnitude > stopDistance)
        {
            // Avancer
            Vector3 move = direction.normalized * (speed * Time.deltaTime);
            transform.position += move;

            // Tourner vers la cible (smoothly)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
