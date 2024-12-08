using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projecting : MouvementState
{
    protected StateMachine stateMachine;
    private Ray _cameraRaycastProjection;
    RaycastHit _cameraHitProjection;
    private float _distance;

    public Projecting(StateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter Projecting");
        base.Enter();
        // tous les controle sont ici plutot que dans l'input de projection car les variable ne sont pas save et on doit tout refaire ici
        
        _cameraRaycastProjection = _sm.gameManager.playerCamScript.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_cameraRaycastProjection, out _cameraHitProjection, _sm.gameManager.range, _sm.gameManager.hitLayer);

        if (_cameraHitProjection.collider != null && _sm.gameManager.projectionTimer > 0)
        {
            _distance = Vector3.Distance(_cameraHitProjection.collider.transform.position, _sm.gameObject.transform.position);
            float reduction = _distance * _sm.gameManager.coeffReducDistance;
            
            //simulation pour pas retirer du temps alors qu'on sortira direct de la projection
            if (_sm.gameManager.projectionTimer - reduction > 0)
            {
                // on retire du temps de projection proportionnelle à la distance 
                _sm.gameManager.projectionTimer -= reduction;

                _sm.gameManager.etatIsProjected = true;
                _sm.gameManager.moveCamScript.ChangeFocusTarget(_cameraHitProjection.collider.transform);
            }
            else
            {
                _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
            }
        }
        else
        {
            _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
        }
    }
    public override void TickLogic()
    {
        base.TickLogic();
        if (_sm.gameManager.projectionTimer <= 0)
        {
            _sm.ChangeState(PlayerMouvementStateMachine.walkingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exit Projecting");
        ResetProjection();
    }
    
    public void ResetProjection()
    {
        _sm.gameManager.etatIsProjected = false;
        _sm.gameManager.moveCamScript.ChangeFocusTarget(); //pos cam player
    }
}
