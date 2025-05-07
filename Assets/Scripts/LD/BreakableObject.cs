using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scindée de l'objet a instantier lors de sa destruction")]
    [SerializeField] private GameObject shatteredVersion;
    [Tooltip("Énergie cinétique minimum à partir duquel une collision détruit l'objet (Ec = m/2 * v²)")]
    [SerializeField] private float minShatterPower;

    private Rigidbody thisRb;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
    private bool hasShattered;

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
            if (!hasShattered)
            {
                hasShattered = true;
                Debug.Log("Shatter Update");
                ShatterObject();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null)
        {
            return;
        }

        Rigidbody rb = collision.rigidbody;
        float cineticForce = (rb.mass / 2) * (rb.velocity.magnitude * rb.velocity.magnitude);

        if (cineticForce >= minShatterPower)
        {
            if (!hasShattered)
            {
                hasShattered = true;
                Debug.Log("Shatter Enter from: " + collision.gameObject.name);
                ShatterObject();
            }
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
