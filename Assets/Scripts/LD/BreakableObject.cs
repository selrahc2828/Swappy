using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Tooltip("La copie scind�e de l'objet a instantier lors de sa destruction")]
    [SerializeField] private GameObject shatteredVersion;
    [Tooltip("�nergie cin�tique minimum � partir duquel une collision d�truit l'objet (Ec = m/2 * v�)")]
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
