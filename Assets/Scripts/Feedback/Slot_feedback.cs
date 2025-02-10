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

    // Update is called once per frame
    public void Feedback_Slot_Changed(Transform spawnPosition = null, Transform targetPosition = null)
    {
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
                        // Destroy(feedback_Act);
                        feedback_Act.GetComponent<FlareMoveTarget>().isAttribute = true;
                    }
                    break;
                case 1:
                    feedback_Act = SpawnFlare(comportementManager.flareSlotImpulse, spawnPosition, targetPosition);
                    break;

                case 3:
                    feedback_Act = SpawnFlare(comportementManager.flareSlotImpulseBouncing, spawnPosition, targetPosition);
                    break;

                case 9:
                    feedback_Act = SpawnFlare(comportementManager.flareSlotImmuable, spawnPosition, targetPosition);
                    break;

                case 27:
                    feedback_Act = SpawnFlare(comportementManager.flareSlotMagnet, spawnPosition, targetPosition);
                    break;

                case 81:
                    feedback_Act = SpawnFlare(comportementManager.flareSlotRocket, spawnPosition, targetPosition);
                    break;

            }
    }

    public GameObject SpawnFlare(GameObject prefabFlare, Transform startPosition, Transform targetPosition)
    {
        if (feedback_Act)
        {
            Destroy(feedback_Act);
        }
        Debug.Log($"startPosition = {startPosition.gameObject.name} \n" +
                  $"targetPosition = {targetPosition.gameObject.name}");
        
        GameObject flareObj = Instantiate(prefabFlare, startPosition.position, Quaternion.identity);
        // flareObj.GetComponent<FlareMoveTarget>().spawnPosition = startPosition;
        flareObj.GetComponent<FlareMoveTarget>().target = targetPosition;
        flareObj.GetComponent<FlareMoveTarget>().SetJourneyLenght();
        
        return flareObj;
    }
}
