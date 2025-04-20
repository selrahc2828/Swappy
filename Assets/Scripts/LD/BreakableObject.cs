using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scindée de l'objet a instantier lors de sa destruction")]
    [SerializeField] private GameObject shatteredVersion;
    [Tooltip("Énergie cinétique minimum à partir duquel une collision détruit l'objet (Ec = m/2 * v²)")]
    [SerializeField] private float minBreakPower;

    private void OnCollisionEnter(Collision collision)
    {
        float cineticForce = (collision.rigidbody.mass/2) * (collision.relativeVelocity.magnitude * collision.relativeVelocity.magnitude);

        if (cineticForce >= minBreakPower)
        {
            GameObject.Instantiate(shatteredVersion, transform.position, transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }
}
