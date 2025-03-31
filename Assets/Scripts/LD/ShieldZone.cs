using UnityEngine;

public class ShieldZone : MonoBehaviour
{
    [SerializeField] private float BounceBackTreshold;
    [SerializeField] private float BounceBackRatio;
    [SerializeField] private AnimationCurve VelocityAttenuationCurve;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            other.attachedRigidbody.velocity = BounceBackObject(other.attachedRigidbody.velocity, other.transform.position);
        }    
    }

    private Vector3 BounceBackObject(Vector3 objectVelocity, Vector3 objectPosition)
    {
        Vector3 normal = (objectPosition - transform.position).normalized;
        Vector3 dir = objectVelocity.normalized;
        float power = objectVelocity.magnitude;
   
        if (objectVelocity.magnitude < BounceBackTreshold)
        {
              power = power * VelocityAttenuationCurve.Evaluate(power/BounceBackTreshold);
        }
        else
        {
            dir = Vector3.Reflect(dir, normal);
            power = power / BounceBackRatio;
        }

        return dir * power;
    }
}
