using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPlanet : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> planetesRB;
    [SerializeField] private Vector3 offsetRotation;
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.planeteCores.Count; i++)
        {
            planetesRB.Add(GameManager.Instance.planeteCores[i].GetComponent<Rigidbody>());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 gravityDirection = CalculateGravityDirection();
        Quaternion targetRotation = Quaternion.FromToRotation(-transform.up, gravityDirection) * transform.rotation;
        targetRotation *= Quaternion.Euler(offsetRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.fixedDeltaTime);
    }
    Vector3 CalculateGravityDirection()
    {
        Vector3 gravityVector = Vector3.zero;
        foreach (Rigidbody planeteRB in planetesRB)
        {
            Vector3 gravitationalForce = (planeteRB.transform.position - transform.position).normalized;
            gravityVector += gravitationalForce;
        }
        return gravityVector;
    }
}
