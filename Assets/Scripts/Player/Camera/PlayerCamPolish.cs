using UnityEngine;

public class PlayerCamPolish : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject cameraHandle;
    [SerializeField] private GameObject armsHandle;

    [Header("Jump Curves")]
    [SerializeField] private float camOnJump;
    [SerializeField] private float camOnJumpTime;
    [SerializeField] private AnimationCurve camOnJumpCurve;
    [Space(8)]
    [SerializeField] private float camOffJumpTime;
    [SerializeField] private AnimationCurve camOffJumpCurve;

    [Header ("Arm Curves")]
    [SerializeField] private float armRailRatio;
    [SerializeField] private float armRailMaxOffset;
    [SerializeField] private AnimationCurve armRailCurve;

    [Header ("Collision Curves")]
    [SerializeField] private float camOnGround;
    [SerializeField] private float camOnGroundTime;
    [SerializeField] private float camOnGroundMax;
    [SerializeField] private float camOnGroundMin;
    [SerializeField] private float camOnGroundForceRatio;
    [SerializeField] private AnimationCurve camOnGroundCurve;
    [Space(8)]
    [SerializeField] private float camOnWall;
    [SerializeField] private float camOnWallTime;
    [SerializeField] private AnimationCurve camOnWallCurve;

    private ControllerPlanete playerController;
    private Rigidbody playerRb;
    private Vector3 lastVelocity;
    private Vector3 currentVelocity;

    private Vector3 baseCamHandlePos;
    private Vector3 baseArmHandlePos;
    private float baseCameraFOV;

    private Vector3 mergedCamOffset;
    private Vector3 lastCameraPos;

    private void Start()
    {
        playerController = player.GetComponent<ControllerPlanete>();
        playerRb = player.GetComponent<Rigidbody>();

        baseCamHandlePos = cameraHandle.transform.localPosition;
        baseArmHandlePos = armsHandle.transform.localPosition;
        baseCameraFOV = playerCamera.fieldOfView;
    }

    private void FixedUpdate()
    {
        lastVelocity = currentVelocity;
        currentVelocity = playerRb.velocity;
    }

    private void Update()
    {
        CameraCurvesTick();
    }


    private void CameraCurvesTick()
    {
        Vector3 cameraOffset1 = Vector3.zero;
        Vector3 cameraOffset2 = Vector3.zero;
        Vector3 cameraOffset3 = Vector3.zero;
        Vector3 cameraOffset4 = Vector3.zero;
        Vector3 cameraOffset5 = Vector3.zero;

        if (isCamOnJumpActive)
        {
            cameraOffset1 = CameraOffsetOnJumpTick();
        }
        if (isCamOffJumpActive)
        {
            cameraOffset2 = CameraOffsetOffJumpTick();
        }
        if (isCamOnWallActive)
        {
            cameraOffset3 = CameraOffsetOnWallTick();
        }
        if (isCamOnGroundActive)
        {
            cameraOffset4 = CameraOffsetOnGroundTick();
        }

        lastCameraPos = cameraHandle.transform.localPosition;
        mergedCamOffset = cameraOffset1 + cameraOffset2 + cameraOffset3 + cameraOffset4 + cameraOffset5;
        cameraHandle.transform.localPosition = mergedCamOffset + baseCamHandlePos;

        Vector3 mergedArmOffset = new Vector3(0, mergedCamOffset.y * armRailRatio, 0);
        armsHandle.transform.localPosition = -mergedArmOffset;

    }


    #region CamOnJump

    private Vector3 camOnJumpPointA;
    private Vector3 camOnJumpPointB;
    private Vector3 camOnJumpLast;
    private float camOnJumpTimer;
    private bool isCamOnJumpActive;
    public void CameraOffsetOnJumpStart()
    {
        isCamOnJumpActive = true;
        camOnJumpTimer = 0;
        camOnJumpPointA = Vector3.zero;
        camOnJumpPointB = new Vector3(0, -camOnJump, 0);
    }

    private Vector3 CameraOffsetOnJumpTick()
    {
        camOnJumpTimer += Time.deltaTime;
        Mathf.Max(camOnJumpTimer, camOnJumpTime);
        camOnJumpLast = Vector3.Lerp(camOnJumpPointA, camOnJumpPointB, camOnJumpCurve.Evaluate(camOnJumpTimer / camOnJumpTime));
        return camOnJumpLast;
    }

    public void CameraOffsetOnJumpEnd()
    {
        isCamOnJumpActive = false;
    }
    #endregion


    #region CamOffJump

    private Vector3 camOffJumpPointA;
    private Vector3 camOffJumpPointB;
    private float camOffJumpTimer;
    private bool isCamOffJumpActive;
    public void CameraOffsetOffJumpStart()
    {
        isCamOffJumpActive = true;
        camOffJumpTimer = 0;
        camOffJumpPointA = camOnJumpLast;
        camOffJumpPointB = Vector3.zero;
    }

    private Vector3 CameraOffsetOffJumpTick()
    {
        camOffJumpTimer += Time.deltaTime;
        if (camOffJumpTimer > camOnJumpTime)
        {
            isCamOffJumpActive = false;
        }
        Vector3 nextCamPos = Vector3.Lerp(camOffJumpPointA, camOffJumpPointB, camOffJumpCurve.Evaluate(camOffJumpTimer / camOnJumpTimer));
        return nextCamPos;
    }

    public void CameraOffsetOffJumpEnd()
    {
        isCamOffJumpActive = false;
    }
    #endregion


    #region CamOnGround

    private Vector3 camOnGroundPointA;
    private Vector3 camOnGroundPointB;
    private float camOnGroundTimer;
    private float camOnGroundForce;
    private bool isCamOnGroundActive;
    public void CameraOffsetOnGroundStart(Collision collision)
    {
        isCamOnGroundActive = true;
        camOnGroundForce = Mathf.Clamp(collision.relativeVelocity.magnitude * 0.1f, 1f, 2f);
        camOnGroundTimer = 0;
        camOnGroundPointA = Vector3.zero;
        camOnGroundPointB = -Vector3.Lerp(collision.contacts[0].normal, transform.up, 0.8f).normalized * camOnGround;

        Debug.Log("VelocityForce: " + collision.relativeVelocity.magnitude);
        Debug.Log("CamForce: " + camOnGroundForce);
    }

    private Vector3 CameraOffsetOnGroundTick()
    {
        camOnGroundTimer += Time.deltaTime;
        if (camOnGroundTimer > camOnGroundTime * Mathf.Clamp(camOnGroundForce * 0.1f, 0.5f, 2f))
        {
            isCamOnGroundActive = false;
        }
        Mathf.Max(camOnGroundTimer, camOnGroundTime);
        Vector3 camOnGroundLast = Vector3.Lerp(camOnGroundPointA, camOnGroundPointB, camOnGroundCurve.Evaluate(camOnGroundTimer / camOnGroundTime));
        return camOnGroundLast * camOnGroundForce;
    }

    public void CameraOffsetOnGroundEnd()
    {
        isCamOnGroundActive = false;
    }
    #endregion


    #region CamOnWall

    private Vector3 camOnWallPointA;
    private Vector3 camOnWallPointB;
    private float camOnWallTimer;
    private float camOnWallForce;
    private bool isCamOnWallActive;
    public void CameraOffsetOnWallStart(Collision collision)
    {
        isCamOnWallActive = true;
        camOnWallTimer = 0;
        camOnWallForce = collision.relativeVelocity.magnitude;
        camOnWallPointB = collision.contacts[0].normal.normalized * camOnWall;
        camOnWallPointA = -collision.contacts[0].normal.normalized * camOnWall;
    }

    private Vector3 CameraOffsetOnWallTick()
    {
        camOnWallTimer += Time.deltaTime;
        if (camOnWallTimer > camOnWallTime)
        {
            isCamOnWallActive = false;
        }
        Mathf.Max(camOnWallTimer, camOnWallTime);
        Vector3 camOnWallLast = Vector3.Lerp(camOnWallPointA, camOnWallPointB, camOnWallCurve.Evaluate(camOnWallTimer / camOnWallTime));
        return camOnWallLast * camOnWallForce;
    }

    public void CameraOffsetOnWallEnd()
    {
        isCamOnWallActive = false;
    }
    #endregion
}
