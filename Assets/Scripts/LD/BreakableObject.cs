using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scindee de l'objet a instantier lors de sa destruction")]
    [SerializeField] private GameObject shatteredVersion;
    [Tooltip("energie cinetique minimum a partir duquel une collision detruit l'objet (Ec = m/2 * v�)")]
    [SerializeField] private float minShatterPower;

    private Rigidbody thisRb;
    private Vector3 previousVelocity;
    private Vector3 currentVelocity;
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
        previousVelocity = currentVelocity;
        currentVelocity = thisRb.velocity;

        if (thisRb.mass * thisRb.velocity.magnitude * Vector3.Angle(currentVelocity, previousVelocity) / 3 >= minShatterPower)
        {
            if (!hasShattered)
            {
                hasShattered = true;
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
        GameObject shatteredObject = Instantiate(shatteredVersion, transform.position, transform.rotation);
        Rigidbody[] shatteredRbs = shatteredObject.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in shatteredRbs)
        {
            rb.velocity = currentVelocity;
        }

        if (_spawner is not null)
        {
            GlobalEventManager.Instance.BrokenPot(_spawner);
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
