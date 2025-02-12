using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_feedback : MonoBehaviour
{

    public bool left_Arm = false;
    public Transform positionFB;
    
    private ComportementManager comportementManager;
    public ComportementStealer_proto comp_steler_proto;
    private GameObject feedback_Act;
    
    //public Transform Arm_transform;    c'est ce qui servira a faire que le feedback suive la main 
    
    private void Start()
    {
        comportementManager = ComportementManager.Instance;
        //comp_steler_proto = GameObject.FindGameObjectWithTag("Player").GetComponent<ComportementStealer_proto>();
    }

    public void Feedback_Slot_Changed(Transform spawnPosition = null, Transform targetPosition = null, bool destroy = false)
    {
        if (destroy)
        {
            // apply sur player ou slot vidé
            Destroy(feedback_Act);
            return;
        }
        
        if (spawnPosition == null)// cas où on donne le comp
            spawnPosition = positionFB;
        
        if (targetPosition == null)// cas où on vole le comp
            targetPosition = positionFB;
        
        int slot = left_Arm ? comp_steler_proto.slot1 : comp_steler_proto.slot2;
        
        switch (slot)
            {
                case 0:
                    if (feedback_Act != null)
                    {
                        feedback_Act.GetComponent<FlareMoveTarget>().startPosition = spawnPosition.position;
                        feedback_Act.GetComponent<FlareMoveTarget>().target = targetPosition;
                        feedback_Act.GetComponent<FlareMoveTarget>().SetDistance();
                        
                        feedback_Act.GetComponent<FlareMoveTarget>().attributeToObject = true;
                    }
                    break;
                case 1:
                    feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
                    feedback_Act.GetComponent<FlareMoveTarget>().flareRenderer.material = comportementManager.flareData.matFlareImpulse;
                    
                    ParticleSystem.MainModule main = feedback_Act.GetComponent<FlareMoveTarget>().flare.main;
                    main.startColor = comportementManager.flareData.particleFlareColorImpulse;
                    break;

                case 3:
                    feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
                    feedback_Act.GetComponent<FlareMoveTarget>().flareRenderer.material = comportementManager.flareData.matFlareBounce;
                    
                    ParticleSystem.MainModule main2 = feedback_Act.GetComponent<FlareMoveTarget>().flare.main;
                    main2.startColor = comportementManager.flareData.particleFlareColorBounce;
                    break;

                case 9:
                    feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
                    feedback_Act.GetComponent<FlareMoveTarget>().flareRenderer.material = comportementManager.flareData.matFlareImmuable;

                    ParticleSystem.MainModule main3 = feedback_Act.GetComponent<FlareMoveTarget>().flare.main;
                    main3.startColor = comportementManager.flareData.particleFlareColorImmuable;
                    break;

                case 27:
                    feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
                    feedback_Act.GetComponent<FlareMoveTarget>().flareRenderer.material = comportementManager.flareData.matFlareMagnet;

                    ParticleSystem.MainModule main4 = feedback_Act.GetComponent<FlareMoveTarget>().flare.main;
                    main4.startColor = comportementManager.flareData.particleFlareColorMagnet;
                    break;

                case 81:
                    feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
                    feedback_Act.GetComponent<FlareMoveTarget>().flareRenderer.material = comportementManager.flareData.matFlareRocket;

                    ParticleSystem.MainModule main5 = feedback_Act.GetComponent<FlareMoveTarget>().flare.main;
                    main5.startColor = comportementManager.flareData.particleFlareColorRocket;
                    break;

            }
    }

    public GameObject SpawnFlare(GameObject prefabFlare, Transform startPosition, Transform targetPosition)
    {
        if (feedback_Act)
        {
            Destroy(feedback_Act);
        }

        GameObject flareObj = Instantiate(prefabFlare, startPosition.position, Quaternion.identity);
        flareObj.GetComponent<FlareMoveTarget>().target = targetPosition;
        flareObj.GetComponent<FlareMoveTarget>().SetDistance();
        flareObj.GetComponent<FlareMoveTarget>().speedCurve = comportementManager.flareData.speedCurve;
        flareObj.GetComponent<FlareMoveTarget>().speed = comportementManager.flareData.speed;
        
        return flareObj;
    }
}
