using UnityEngine;

public class PlayerPhysicsPolish : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Speed Curves")]
    [SerializeField] private float maxSpeedOnJump;
    [SerializeField] private AnimationCurve maxSpeedOnJumpCurve;
    [SerializeField] private float maxSpeedOffJumpTime;
    [SerializeField] private AnimationCurve maxSpeedOffJumpCurve;
    [Space(8)]
    [SerializeField] private float maxSpeedOnFall;
    [SerializeField] private float maxSpeedOnFallTime;
    [SerializeField] private AnimationCurve maxSpeedOnFallCurve;
    [SerializeField] private AnimationCurve maxSpeedOnFallRepartitionCurve;

    [Header("Air Curves")]
    [SerializeField] private float airControlOnAirMin;
    [SerializeField] private float airControlOnAirMax;
    [SerializeField] private float airControlOnAirTime;
    [SerializeField] private AnimationCurve airControlOnAirCurve;

    private ControllerPlanete pControler;
    private Rigidbody pRb;
    private Vector3 lastVelocity;
    private Vector3 currentVelocity;

    private float baseMaxSpeed;

    private void Start()
    {
        pControler = player.GetComponent<ControllerPlanete>();
        pRb = player.GetComponent<Rigidbody>();
        baseMaxSpeed = pControler.maxSpeed;
    }

    private void FixedUpdate()
    {
        lastVelocity = currentVelocity;
        currentVelocity = pRb.velocity;
    }
    private void Update()
    {
        ControllerCurvesTick();
    }

    private void ControllerCurvesTick()
    {
        if (maxSpeedOnJumpActive)
        {
            pControler.maxSpeed = MaxSpeedOnJumpTick(pControler.jumpTime);
        }
        if (maxSpeedOffJumpActive)
        {
            pControler.maxSpeed = MaxSpeedOffJumpTick(pControler.jumpTime);
        }
        if (airControlOnAirActive)
        {
            pControler.airControlMultiplier = AirControlOnAirTick();
        }
    }

    #region SpeedReducOnJump

    private float maxSpeedOnJumpTimer;
    private bool maxSpeedOnJumpActive;
    public void MaxSpeedOnJumpStart()
    {
        maxSpeedOnJumpActive = true;
        maxSpeedOnJumpTimer = 0;
    }

    public float MaxSpeedOnJumpTick(float jumpTime)
    {
        maxSpeedOnJumpTimer += Time.deltaTime;
        Mathf.Max(maxSpeedOnJumpTimer, jumpTime);
        float newMaxSpeed = Mathf.Lerp(baseMaxSpeed, maxSpeedOnJump, maxSpeedOnJumpCurve.Evaluate(maxSpeedOnJumpTimer / jumpTime));
        return newMaxSpeed;
    }

    public void MaxSpeedOnJumpEnd()
    {
        maxSpeedOnJumpActive = false;
    }
    #endregion


    #region SpeedReducOffJump
    private float maxSpeedOffJumpTimer;
    private bool maxSpeedOffJumpActive;
    public void MaxSpeedOffJumpStart()
    {
        maxSpeedOffJumpActive = true;
        maxSpeedOffJumpTimer = 0;
    }

    public float MaxSpeedOffJumpTick(float jumpTime)
    {
        maxSpeedOffJumpTimer += Time.deltaTime;
        if (maxSpeedOffJumpTimer > maxSpeedOffJumpTime)
        {
            maxSpeedOffJumpActive = false;
            return baseMaxSpeed;
        }
        Mathf.Max(maxSpeedOffJumpTimer, jumpTime);
        float newMaxSpeed = Mathf.Lerp(maxSpeedOnJump, baseMaxSpeed, maxSpeedOffJumpCurve.Evaluate(maxSpeedOffJumpTimer / jumpTime));
        return newMaxSpeed;
    }

    public void MaxSpeedOffJumpEnd()
    {
        maxSpeedOffJumpActive = false;
    }
    #endregion


    #region MaxSpeedOnFall

    private float maxSpeedOnFallTimer;
    private bool maxSpeedOnFallActive;
    public void MaxSpeedOnFallStart()
    {
        maxSpeedOnFallActive = true;
        maxSpeedOnFallTimer = 0;
    }

    public float MaxSpeedOnFallTick(float jumpTime)
    {
        return 0;
    }

    public void MaxSpeedOnFallEnd()
    {
        maxSpeedOnFallActive = false;
    }

    #endregion


    #region AirControlOnAir

    private bool airControlOnAirActive;
    private float airControlOnAirTimer;
    public void AirControlOnAirStart()
    {
        airControlOnAirActive = true;
        airControlOnAirTimer = 0;
    }

    public float AirControlOnAirTick()
    {
        airControlOnAirTimer += Time.deltaTime;
        if (airControlOnAirTimer > airControlOnAirTime)
        {
            airControlOnAirActive = false;
        }
        Mathf.Max(airControlOnAirTimer, airControlOnAirTime);
        float newAirControl = Mathf.Lerp(airControlOnAirMin, airControlOnAirMax, airControlOnAirCurve.Evaluate(airControlOnAirTimer / airControlOnAirTime));
        Debug.Log("AirControl: " + newAirControl);
        return newAirControl;
    }

    public void AirControlOnAirEnd()
    {
        airControlOnAirActive = false;
        pControler.airControlMultiplier = airControlOnAirMin;
    }
    #endregion
}