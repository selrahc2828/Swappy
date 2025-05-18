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

    public float maxSpeed;
    public float moveSpeed;
    public float sprintMultiplier;
    public float airControlMultiplier;
    public float stoppingRatio;
    public float sideSpeedReductionRatio;
    public float jumpForceMIN;
    public float jumpForceMAX;
    public float jumpTime;
    public float coyoteeTime;
    public float jumpCooldown;
    public float wallBumpRatio;

    private float jumpTimer;
    private bool isChargingJump;

    private float coyoteeTimer;
    private bool hasJumped;

    [Header("References")]
    [SerializeField] private PlayerCamPolish cameraPolish;
    [SerializeField] private PlayerPhysicsPolish physicsPolish;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject cameraHandle;
    [SerializeField] private GameObject armsHandle;

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
            wallBumpVelocity = wallBumpVelocity / 1.05f;
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
            cameraPolish.CameraOffsetOnGroundStart(collision);
        }
        else
        {
            cameraPolish.CameraOffsetOnWallStart(collision);
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
            cameraPolish.CameraOffsetOnJumpStart();
            physicsPolish.MaxSpeedOnJumpStart();
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
        isChargingJump = false;
        hasAlreadyJumped = true;
        jumpCooldownTimer = 0;

        Mathf.Max(jumpTimer, jumpTime);
        float jumpForce = Mathf.Lerp(jumpForceMIN, jumpForceMAX, jumpTimer / jumpTime);
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);

        cameraPolish.CameraOffsetOnJumpEnd();
        cameraPolish.CameraOffsetOffJumpStart();
        physicsPolish.MaxSpeedOffJumpStart();
        return;
    }
}
