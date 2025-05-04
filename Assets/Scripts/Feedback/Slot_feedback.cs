using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_feedback : MonoBehaviour
{

    public bool right_Arm = false;
    public Transform positionFB;
    
    private ComportementManager comportementManager;
    private ComportementStealer_proto comp_steler_proto;
    private GameObject feedback_Act;

    //public Transform Arm_transform;    c'est ce qui servira a faire que le feedback suive la main 

    private void OnEnable()
    {
        GlobalEventManager.Instance.OnComportmentAdded += RecieveAddedSignal;
        GlobalEventManager.Instance.OnComportmentExtracted+= RecieveExtractedSignal;
        GlobalEventManager.Instance.OnComportmentExchanged += RecieveExchangedSignal;
    }

    private void Start()
    {
        comportementManager = ComportementManager.Instance;
        comp_steler_proto = GameManager.Instance.player.GetComponent<ComportementStealer_proto>();
    }

    private void OnDisable()
    {
        GlobalEventManager.Instance.OnComportmentAdded -= RecieveAddedSignal;
        GlobalEventManager.Instance.OnComportmentExtracted -= RecieveExtractedSignal;
        GlobalEventManager.Instance.OnComportmentExchanged -= RecieveExchangedSignal;
    }

    public void RecieveAddedSignal(GameObject objectToAddComportment, bool rightValue, bool rightHand)
    {
        if (rightHand == right_Arm)
        {
            if (objectToAddComportment.CompareTag("Player"))
            {
                Feedback_Slot_Changed(null, objectToAddComportment.transform, true);
            }
            else
            {
                Feedback_Slot_Changed(null, objectToAddComportment.transform, false);
            }
        }
    }
    
    public void RecieveExtractedSignal(GameObject originOfComportment, bool rightValue, bool rightHand)
    {
        if (rightHand == right_Arm)
        {
            Feedback_Slot_Changed(originOfComportment.transform, null, false);
        }
    }

    public void RecieveExchangedSignal(GameObject player, bool rightHand)
    {
        //je sais pas gérer l'échange
    }

    public void Feedback_Slot_Changed(Transform spawnPosition = null, Transform targetPosition = null, bool destroy = false)
    {
        if (destroy)
        {
            // apply sur player ou slot vidé
            Destroy(feedback_Act);
            return;
        }
        
        // ??= si pas de valeur, on prend positionFB (equivalent if null)
        spawnPosition ??= positionFB; 
        targetPosition ??= positionFB;
        
        int slot = right_Arm ? comp_steler_proto.slot2 : comp_steler_proto.slot1;

        // si slot vide, on "detruit" la prefab donc sert pas de passer par le switch
        if (slot == 0)
        {
            if (feedback_Act != null)
            {
                FlareMoveTarget flare = feedback_Act.GetComponent<FlareMoveTarget>();
                flare.startPosition = spawnPosition.position;
                flare.target = targetPosition;
                flare.SetDistance();
                flare.attributeToObject = true;
            }
            return;
        }
        
        // set matérial et couleur du flare
        Material material = null;
        Color flareColor = Color.grey;
        
        switch (slot)
        {
            case 1:
                material = comportementManager.flareData.matFlareImpulse;
                flareColor = comportementManager.flareData.particleFlareColorImpulse;
                break;

            case 3:
                material = comportementManager.flareData.matFlareBounce;
                flareColor = comportementManager.flareData.particleFlareColorBounce;
                break;

            case 9:
                material = comportementManager.flareData.matFlareImmuable;
                flareColor = comportementManager.flareData.particleFlareColorImmuable;
                break;

            case 27:
                material = comportementManager.flareData.matFlareMagnet;
                flareColor = comportementManager.flareData.particleFlareColorMagnet;
                break;

            case 81:
                material = comportementManager.flareData.matFlareRocket;
                flareColor = comportementManager.flareData.particleFlareColorRocket;
                break;
        }
        // instantie Feedback et attribution materail et couleur flare
        feedback_Act = SpawnFlare(comportementManager.flareData.prefabFlareSlotHand, spawnPosition, targetPosition);
        FlareMoveTarget flareMove = feedback_Act.GetComponent<FlareMoveTarget>();
        
        flareMove.flareRenderer.material = material;
        var mainModule = flareMove.flare.main;
        mainModule.startColor = flareColor;
    }

    public GameObject SpawnFlare(GameObject prefabFlare, Transform startPosition, Transform targetPosition)
    {
        if (feedback_Act)
        {
            Destroy(feedback_Act);
        }

        GameObject flareObj = Instantiate(prefabFlare, startPosition.position, Quaternion.identity);
        FlareMoveTarget flare = flareObj.GetComponent<FlareMoveTarget>();
        flare.target = targetPosition;
        flare.SetDistance();
        flare.speedCurve = comportementManager.flareData.speedCurve;
        flare.speed = comportementManager.flareData.speed;
        
        return flareObj;
    }
}
