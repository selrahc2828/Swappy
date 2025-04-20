using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private GameObject shatteredVersion;
    [SerializeField] private float minBreakMagnitude;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.relativeVelocity.magnitude);

        if (collision.relativeVelocity.magnitude > minBreakMagnitude)
        {
            GameObject.Instantiate(shatteredVersion, transform.position, transform.rotation);
            GameObject.Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().centerOfMass, 0.1f);
    }
}
