using System;
using UnityEngine;

public class LargageCapsuleINTRO : MonoBehaviour
{
    public GameObject Capsule;
    public GameObject pinceGauche;
    public GameObject pinceDroite;

    public AnimationCurve pinceCurve;
    private float _t;
    public float timeRange = 1;

    private bool playActived = false;

    public Vector3 pinceGaucheTargetEuler = new Vector3(-90, 0, -15);
    public Vector3 pinceDroiteTargetEuler = new Vector3(90, 0, 160);
    private Vector3 pinceGaucheStartEuler;
    private Vector3 pinceDroiteStartEuler;

    public void LargageCapsule()
    {
        playActived = true;
        Capsule.GetComponent<Rigidbody>().useGravity = true;
        Capsule.GetComponent<Rigidbody>().isKinematic = false;

        // Stockage des angles locaux de départ
        pinceGaucheStartEuler = pinceGauche.transform.localEulerAngles;
        pinceDroiteStartEuler = pinceDroite.transform.localEulerAngles;

        Debug.Log("Start G: " + pinceGaucheStartEuler);
        Debug.Log("Start D: " + pinceDroiteStartEuler);
    }

    private void FixedUpdate()
    {
        if (playActived)
        {
            _t += Time.deltaTime;
            float r = Mathf.Clamp01(_t / timeRange); // on évite de dépasser 1
            float tEval = pinceCurve.Evaluate(r);

            // Interpolation angle par angle avec LerpAngle
            Vector3 gaucheRot = new Vector3(
                Mathf.LerpAngle(pinceGaucheStartEuler.x, pinceGaucheTargetEuler.x, tEval),
                Mathf.LerpAngle(pinceGaucheStartEuler.y, pinceGaucheTargetEuler.y, tEval),
                Mathf.LerpAngle(pinceGaucheStartEuler.z, pinceGaucheTargetEuler.z, tEval)
            );

            Vector3 droiteRot = new Vector3(
                Mathf.LerpAngle(pinceDroiteStartEuler.x, pinceDroiteTargetEuler.x, tEval),
                Mathf.LerpAngle(pinceDroiteStartEuler.y, pinceDroiteTargetEuler.y, tEval),
                Mathf.LerpAngle(pinceDroiteStartEuler.z, pinceDroiteTargetEuler.z, tEval)
            );

            // Application en local
            pinceGauche.transform.localRotation = Quaternion.Euler(gaucheRot);
            pinceDroite.transform.localRotation = Quaternion.Euler(droiteRot);
        }
    }
}
