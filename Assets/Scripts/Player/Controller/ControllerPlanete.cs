using AmplifyShaderEditor;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Input;

[RequireComponent(typeof(Rigidbody))]
public class ControllerPlanete : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;
    private Vector2 moveInputVector;
    private Vector3 moveDirection;
    private Controls controls;
    private Transform orientation;
    private float playerHeight;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float airControlMultiplier;
    [SerializeField] private float stoppingRatio;
    [SerializeField] private float sideSpeedReductionRatio;
    [SerializeField] private float jumpForceMIN;
    [SerializeField] private float jumpForceMAX;
    [SerializeField] private float jumpTime;
    [SerializeField] private float coyoteeTime;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float wallBumpRatio;

    private float jumpTimer;
    private bool isChargingJump;

    private float coyoteeTimer;
    private bool hasJumped;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject cameraHandle;
    [SerializeField] private GameObject armsHandle;

    private Vector3 baseCamHandlePos;
    private float baseCameraFOV;
    private float baseMaxSpeed;
    private Vector3 baseArmHandlePos;

    [Header("Physics Curves")]
    private AnimationCurve walkSpeedOnMoveCurve;
    private AnimationCurve walkSpeedOffMoveCurve;

    private float maxSpeedOnJump;
    private AnimationCurve maxSpeedOnJumpCurve;
    private float maxSpeedOffJumpTime;
    private AnimationCurve maxSpeedOffJumpCurve;

    private float airControlMinimum;
    private AnimationCurve airControlCurve;

    [Header("Camera Curves")]
    private Vector3 mergedCamOffset;

    [SerializeField] private float camOnJump;
    [SerializeField] private float camOnJumpTime;
    [SerializeField] private AnimationCurve camOnJumpCurve;

    [SerializeField] private float camOffJumpTime;
    [SerializeField] private AnimationCurve camOffJumpCurve;

    [SerializeField] private float armRailRatio;
    [SerializeField] private float armRailMaxOffset;
    [SerializeField] private AnimationCurve armRailCurve;

    [SerializeField] private float camOnGround;
    [SerializeField] private float camOnGroundTime;
    [SerializeField] private AnimationCurve camOnGroundCurve;

    [SerializeField] private float camOnWall;
    [SerializeField] private float camOnWallTime;
    [SerializeField] private AnimationCurve camOnWallCurve;

    [Space(16)]
    [Header("Debug")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isStopping;
    [SerializeField] private bool grounded;
    [SerializeField] private bool touchingInclinedSurface;

    private LayerMask whatIsGround;
    private GravityPlanete gravityComponent;
    [SerializeField] private Vector3 touchingInclinedSurfaceDirection;

    private void OnEnable()
    {
        gameManager = GameManager.Instance;
        controls = GameManager.controls;

        controls.Player.Movement.performed += MovementAttack;
        controls.Player.Movement.canceled += MovementAttack;
        controls.Player.Jump.performed += StartJump;
        controls.Player.Jump.canceled += EndJump;
        controls.Player.StartSprint.performed += StartSprint;
        controls.Player.StopSprint.performed += StopSprint;

        maxSpeed = gameManager.maxSpeed;
        moveSpeed = gameManager.moveSpeed;
        sprintMultiplier = gameManager.sprintMultiplier;
        airControlMultiplier = gameManager.airControlMultiplier;
        stoppingRatio = gameManager.stoppingRatio;
        sideSpeedReductionRatio = gameManager.sideSpeedReductionRatio;
        jumpForceMAX = gameManager.jumpForceMAX;
        jumpForceMIN = gameManager.jumpForceMIN;
        jumpTime = gameManager.jumpTime;
        coyoteeTime = gameManager.coyoteeTime;
        jumpCooldown = gameManager.jumpCooldown;
        wallBumpRatio = gameManager.wallBumpRatio;

        playerHeight = gameManager.playerHeight;
        whatIsGround = gameManager.whatIsGround;

        walkSpeedOnMoveCurve = gameManager.walkSpeedOnMoveCurve;
        walkSpeedOffMoveCurve = gameManager.walkSpeedOffMoveCurve;

        maxSpeedOnJump = gameManager.maxSpeedOnJump;
        maxSpeedOnJumpCurve = gameManager.maxSpeedOnJumpCurve;
        maxSpeedOffJumpTime = gameManager.maxSpeedOffJumpTime;
        maxSpeedOffJumpCurve = gameManager.maxSpeedOffJumpCurve;

        airControlMinimum = gameManager.airControlMultiplierMinimum;
        airControlCurve = gameManager.airControlMultiplierCurve;

        camOnJump = gameManager.cameraOffsetOnJump;
        camOnJumpTime = gameManager.cameraOffsetOnJumpTime;
        camOnJumpCurve = gameManager.cameraOffsetOnJumpCurve;

        camOffJumpTime = gameManager.cameraOffsetOffJumpTime;
        camOffJumpCurve = gameManager.cameraOffsetOffJumpCurve;

        camOnGround = gameManager.cameraOffsetOnGround;
        camOnGroundTime = gameManager.cameraOffsetOnGroundTime;
        camOnGroundCurve = gameManager.cameraOffsetOnGroundCurve;

        camOnWall = gameManager.cameraOffsetOnWall;
        camOnWallTime = gameManager.cameraOffsetOnWallTime;
        camOnWallCurve = gameManager.cameraOffsetOnWallCurve;

        armRailRatio = gameManager.armRailRatio;
        armRailMaxOffset = gameManager.armRailMaxOffset;
        armRailCurve = gameManager.armRailCurve;
    }

    private void OnDisable()
    {
        controls.Player.Movement.performed -= MovementAttack;
        controls.Player.Movement.canceled -= MovementAttack;
        controls.Player.Jump.performed -= StartJump;
        controls.Player.Jump.canceled -= EndJump;
        controls.Player.StartSprint.performed -= StartSprint;
        controls.Player.StopSprint.performed -= StopSprint;
    }

    void Start()
    {
        orientation = GameManager.Instance.orientation;
        isStopping = true;
        rb = GetComponent<Rigidbody>();
        gravityComponent = GetComponent<GravityPlanete>();

        baseCamHandlePos = cameraHandle.transform.localPosition;
        baseCameraFOV = Camera.main.fieldOfView;
        baseMaxSpeed = maxSpeed;
        baseArmHandlePos = armsHandle.transform.localPosition;
    }

    private void Update()
    {
        moveInputVector = controls.Player.Movement.ReadValue<Vector2>().normalized;

        // Calculate movement direction using GravityPlanete
        if (gravityComponent != null)
        {
            moveDirection = Vector3.ProjectOnPlane(
                orientation.forward * moveInputVector.y +
                orientation.right * moveInputVector.x,
                transform.up
            ).normalized;
        }

        //est ce que je touche un mur et l'input de mouvement est vers le mur
        if (touchingInclinedSurface && Vector3.Dot(moveDirection, touchingInclinedSurfaceDirection) < 0)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, touchingInclinedSurfaceDirection);
        }

        if (isChargingJump)
        {
            jumpTimer += Time.deltaTime;
        }

        CameraCurvesTick();
        ControllerCurvesTick();
    }

    private Vector3 lastVelocity;
    private Vector3 currentVelocity;
    private Vector3 wallBumpVelocity;

    void FixedUpdate()
    {
        GroundCheck();

        Vector3 localHorizontalVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);

        float orientationValue = 1;
        float aerialMultiplierValue = grounded ? 1 : airControlMultiplier;
        float sprintMultiplierValue = isSprinting ? sprintMultiplier : 1;

        if (grounded)
        {
            if (rb.velocity.magnitude > 1f)
            {
                orientationValue = ((Vector3.Dot(moveDirection.normalized, rb.velocity.normalized) - 1) / -2) + 1;
            }
            if (moveInputVector.y == 0)
            {
                Vector3 velocityWithoutSides = Vector3.ProjectOnPlane(rb.velocity, orientation.right);

                rb.velocity = Vector3.Lerp(rb.velocity, velocityWithoutSides, sideSpeedReductionRatio);
            }
            if (isStopping)
            {
                rb.velocity *= stoppingRatio;
            }
            wallBumpVelocity = wallBumpVelocity * 0.01f;
        }
        if (localHorizontalVelocity.magnitude > maxSpeed * sprintMultiplierValue && Vector3.Dot(moveDirection, localHorizontalVelocity) > 0)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, localHorizontalVelocity);
        }
        rb.AddForce(moveDirection * (moveSpeed * orientationValue * aerialMultiplierValue * sprintMultiplierValue) + wallBumpVelocity, ForceMode.Acceleration);

        coyoteeTimer += Time.fixedDeltaTime;
        jumpCooldownTimer += Time.fixedDeltaTime;

        lastVelocity = currentVelocity;
        currentVelocity = rb.velocity;
    }

    void GroundCheck()
    {
        Vector3 groundDirection = -transform.up;
        grounded = false;
        touchingInclinedSurface = false;

        if (Physics.Raycast(transform.position, groundDirection, out RaycastHit hit, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            float slopeAngle = Vector3.Angle(hit.normal, -groundDirection);
            if (slopeAngle < 75f) // Seulement consid�r� comme sol si l'angle est faible
            {
                grounded = true;
            }
            coyoteeTimer = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.GetContact(0).thisCollider.CompareTag("AntiStick"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                float slopeAngle = Vector3.Angle(contact.normal, -transform.up);

                if (slopeAngle > 90f && slopeAngle < 125f) // Surface inclin�e d�tect�e
                {
                    touchingInclinedSurface = true;
                    touchingInclinedSurfaceDirection = contact.normal;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GroundCheck();
        if (grounded)
        {
            hasAlreadyJumped = false;
            CameraOffsetOnGroundStart(collision);
        }
        else
        {
            CameraOffsetOnWallStart(collision);
            wallBumpVelocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal) * wallBumpRatio;
        }
    }

    private void MovementAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isStopping = false;
        }
        if (context.canceled)
        {
            isStopping = true;
        }
    }

    private void StartSprint(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            isSprinting = true;
        }
    }
    private void StopSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSprinting = false;
        }
    }

    private void StartJump(InputAction.CallbackContext context)
    {
        if (hasAlreadyJumped)
        {
            return;
        }

        if (jumpCooldownTimer < jumpCooldown)
        {
            return;
        }

        if (context.performed && grounded)
        {
            jumpTimer = 0;
            isChargingJump = true;
            CameraOffsetOnJumpStart();
            MaxSpeedOnJumpStart();
        }
    }

    private void EndJump(InputAction.CallbackContext context)
    {
        if (hasAlreadyJumped)
        {
            return;
        }

        if (jumpCooldownTimer < jumpCooldown)
        {
            return;
        }

        if (grounded)
        {
            JumpAction();
            return;
        }

        if (coyoteeTimer < coyoteeTime)
        {
            JumpAction();
            return;
        }
    }

    private float jumpCooldownTimer;
    private bool hasAlreadyJumped;

    private void JumpAction()
    {
        isCamOnJumpActive = false;
        maxSpeedOnJumpActive = false;
        isChargingJump = false;
        hasAlreadyJumped = true;
        jumpCooldownTimer = 0;

        Mathf.Max(jumpTimer, jumpTime);
        float jumpForce = Mathf.Lerp(jumpForceMIN, jumpForceMAX, jumpTimer / jumpTime);
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);

        CameraOffsetOffJumpStart();
        MaxSpeedOffJumpStart();
        return;
    }

    #region ControllerCurves

    private void ControllerCurvesTick()
    {
        if (maxSpeedOnJumpActive)
        {
           maxSpeed = MaxSpeedOnJumpTick();
        }
        if (maxSpeedOffJumpActive)
        {
           maxSpeed = MaxSpeedOffJumpTick();
        }
    }

    #region SpeedReducOnJump
    private float maxSpeedOnJumpTimer;
    private bool maxSpeedOnJumpActive;
    private void MaxSpeedOnJumpStart()
    {
        maxSpeedOnJumpActive = true;
        maxSpeedOnJumpTimer = 0;
    }

    private float MaxSpeedOnJumpTick()
    {
        maxSpeedOnJumpTimer += Time.deltaTime;
        Mathf.Max(maxSpeedOnJumpTimer, jumpTime);
        float newMaxSpeed = Mathf.Lerp(baseMaxSpeed, maxSpeedOnJump, camOnJumpCurve.Evaluate(maxSpeedOnJumpTimer / jumpTime));
        return newMaxSpeed;
    }


    #endregion

    #region SpeedReducOffJump
    private float maxSpeedOffJumpTimer;
    private bool maxSpeedOffJumpActive;
    private void MaxSpeedOffJumpStart()
    {
        maxSpeedOffJumpActive = true;
        maxSpeedOffJumpTimer = 0;
    }

    private float MaxSpeedOffJumpTick()
    {
        maxSpeedOffJumpTimer += Time.deltaTime;
        if (camOffJumpTimer > camOffJumpTime)
        {
            maxSpeedOffJumpActive = false;
            return baseMaxSpeed;
        }
        Mathf.Max(maxSpeedOffJumpTimer, jumpTime);
        float newMaxSpeed = Mathf.Lerp(maxSpeedOnJump, baseMaxSpeed, camOffJumpCurve.Evaluate(maxSpeedOffJumpTimer / jumpTime));
        return newMaxSpeed;
    }

    #endregion

    #endregion


    #region CameraCurves

    private Vector3 lastCameraPos;
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
    }

    #region CamOnJump

    private Vector3 camOnJumpPointA;
    private Vector3 camOnJumpPointB;
    private Vector3 camOnJumpLast;
    private float camOnJumpTimer;
    private bool isCamOnJumpActive;
    private void CameraOffsetOnJumpStart()
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
        camOnJumpLast = Vector3.Lerp(camOnJumpPointA, camOnJumpPointB, camOnJumpCurve.Evaluate(camOnJumpTimer/camOnJumpTime));
        return camOnJumpLast;
    }

    #endregion

    #region CamOffJump

    private Vector3 camOffJumpPointA;
    private Vector3 camOffJumpPointB;
    private float camOffJumpTimer;
    private bool isCamOffJumpActive;
    private void CameraOffsetOffJumpStart()
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

    #endregion

    #region CamOnGround

    private Vector3 camOnGroundPointA;
    private Vector3 camOnGroundPointB;
    private float camOnGroundTimer;
    private float camOnGroundForce;
    private bool isCamOnGroundActive;
    private void CameraOffsetOnGroundStart(Collision collision)
    {
        isCamOnGroundActive = true;
        camOnGroundForce = Mathf.Clamp(collision.relativeVelocity.magnitude * 0.2f, 1f, 2f);
        camOnGroundTimer = camOnGroundTime * Mathf.Clamp(camOnGroundForce * 0.2f, 0.5f, 2f);
        camOnGroundPointA = Vector3.zero;
        camOnGroundPointB = -Vector3.Lerp(collision.contacts[0].normal, transform.up, 0.8f).normalized * camOnGround;
    }

    private Vector3 CameraOffsetOnGroundTick()
    {
        camOnGroundTimer -= Time.deltaTime;
        if (camOnGroundTimer < 0)
        {
            isCamOnGroundActive = false;
        }
        Mathf.Max(camOnGroundTimer, camOnGroundTime);
        Vector3 camOnGroundLast = Vector3.Lerp(camOnGroundPointA, camOnGroundPointB, camOnGroundCurve.Evaluate(Mathf.Abs(1-camOnGroundTimer) / camOnGroundTime));
        return camOnGroundLast * camOnGroundForce;
    }

    #region CamOnWall

    private Vector3 camOnWallPointA;
    private Vector3 camOnWallPointB;
    private float camOnWallTimer;
    private float camOnWallForce;
    private bool isCamOnWallActive;
    private void CameraOffsetOnWallStart(Collision collision)
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
        if ( camOnWallTimer > camOnWallTime) 
        {
            isCamOnWallActive = false; 
        }
        Mathf.Max(camOnWallTimer, camOnWallTime);
        Vector3 camOnWallLast = Vector3.Lerp(camOnWallPointA, camOnWallPointB, camOnWallCurve.Evaluate(camOnWallTimer / camOnWallTime));
        return camOnWallLast * camOnWallForce;
    }

    #endregion

    #endregion

    #region ArmCurves

    private void ArmCurvesTick() //fonctionne pas, je garde au cas ou
    {
        float armOffset1 = 0;
        float armOffset2 = 0;

        armOffset1 = ArmHandlePhysicsTick();
        armOffset2 = ArmHandleCameraTick();

        float mergedArmOffset = armsHandle.transform.localPosition.y + armOffset1 + armOffset2;
        float finalArmRatio = armRailCurve.Evaluate(mergedArmOffset / armRailMaxOffset);
        float finalArmPos = armRailMaxOffset * finalArmRatio;

        Debug.Log("mergedArmOffset: " + mergedArmOffset);
        Debug.Log("finalArmRatio:" + finalArmRatio);
        Debug.Log("finalArmPos: " + finalArmPos);

        armsHandle.transform.localPosition = (Vector3.up * finalArmPos) + baseArmHandlePos;
    }

    private float ArmHandlePhysicsTick()
    {
        float physicsRange = lastVelocity.y - rb.velocity.y;
        float pullForce = physicsRange * armRailRatio * Time.deltaTime;
        return 0;
    }

    private float ArmHandleCameraTick()
    { 
        float camRange = lastCameraPos.y - cameraHandle.transform.localPosition.y;
        float pullForce = camRange * armRailRatio * Time.deltaTime;
        return 0; 
    }

    #endregion

    #endregion
}
