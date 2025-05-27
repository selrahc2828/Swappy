using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scindee de l'objet a instantier lors de sa destruction")]
    public GameObject shatteredVersion;
    [Tooltip("energie cinetique minimum a partir duquel une collision detruit l'objet (Ec = m/2 * v�)")]
    [SerializeField] private float minShatterPower;
    [Tooltip("Avoid breaking object with regular collisions and force")]
    [SerializeField] private bool avoidCollisionBreak;

    private Rigidbody thisRb;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
    private Vector3 previousAngularVelocity;
    private Vector3 currentAngularVelocity;
    private bool hasShattered;

    private SpawnPot _spawner;
    public SpawnPot Spawner
    {
        set => _spawner = value;
    }

    public GameObject fragmentPrefab;
    
    private void Start()
    {
        thisRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        currentAngularVelocity = thisRb.angularVelocity;
        previousAngularVelocity = thisRb.angularVelocity;

        previousVelocity = currentVelocity;
        currentVelocity = thisRb.velocity;

        if (!avoidCollisionBreak)
        {
            if (thisRb.mass * thisRb.velocity.magnitude * Vector3.Angle(currentVelocity, previousVelocity) / 3 >= minShatterPower)
            {
                if (!hasShattered)
                {
                    hasShattered = true;
                    ShatterObject(currentVelocity, currentAngularVelocity);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!avoidCollisionBreak)
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
                    ShatterObject(currentVelocity, currentAngularVelocity);
                }
            }
        }
    }

    public void ShatterObject(Vector3 newVelocity, Vector3 newAngular)
    {
        GameObject shatteredObject = Instantiate(shatteredVersion, transform.position, transform.rotation);
        Rigidbody[] shatteredRbs = shatteredObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in shatteredRbs)
        {
            rb.velocity = newVelocity;
            rb.angularVelocity = newAngular;
        }

        if (_spawner is not null)
        {
            GlobalEventManager.Instance.BrokenPot();
            ExplodeFragments();
        }

        Destroy(gameObject);
    }


    private void ExplodeFragments()
    {
        for (int i = 0; i < (int)_spawner.potData.tier; i++)
        {
            Instantiate(fragmentPrefab, transform.position + new Vector3(0f,0.5f,0f), Quaternion.identity);
        }
    }
}
