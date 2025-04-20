using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scind�e de l'objet a instantier lors de sa destruction")]
    [SerializeField] private GameObject shatteredVersion;
    [Tooltip("�nergie cin�tique minimum � partir duquel une collision d�truit l'objet (Ec = m/2 * v�)")]
    [SerializeField] private float minShatterPower;

    private Rigidbody thisRb;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;

    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        previousVelocity = currentVelocity;
        currentVelocity = thisRb.velocity;

        if (thisRb.mass * thisRb.velocity.magnitude * Vector3.Angle(currentVelocity, previousVelocity) / 3 >= minShatterPower)
        {
            ShatterObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        float cineticForce = (rb.mass / 2) * (rb.velocity.magnitude * rb.velocity.magnitude);

        if (cineticForce >= minShatterPower)
        {
            ShatterObject();
        }
    }

    private void ShatterObject()
    {
        GameObject shatteredObject = GameObject.Instantiate(shatteredVersion, transform.position, transform.rotation);
        Rigidbody[] shatteredRbs = shatteredObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in shatteredRbs)
        {
            rb.velocity = currentVelocity;
        }

        GameObject.Destroy(gameObject);
    }
}
