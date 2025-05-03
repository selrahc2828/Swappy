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
    public float acceleration = 10f;
    public float maxSpeed = 3f;
    
    private Rigidbody rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // on reste à plat

        if (direction.magnitude > stopDistance)
        {
            // // Avancer
            // Vector3 move = direction.normalized * (speed * Time.deltaTime);
            // transform.position += move;
            //
            // // Tourner vers la cible (smoothly)
            // Quaternion targetRotation = Quaternion.LookRotation(direction);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
            Vector3 desiredVelocity = direction.normalized * speed;
            Vector3 force = (desiredVelocity - rb.velocity) * acceleration;
            
            // Ajout de la force pour atteindre la vitesse voulue
            rb.AddForce(force, ForceMode.Acceleration);
            
            // Limite la vitesse max
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
            
            // Rotation vers la cible
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }else{
            // Freinage si trop proche
            rb.velocity = Vector3.zero; //Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * acceleration);
        }
    }
}
