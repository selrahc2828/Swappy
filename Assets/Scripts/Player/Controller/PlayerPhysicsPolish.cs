using UnityEngine;

public class PlayerPhysicsPolish : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;

    [Header("Speed Curves")]
    [SerializeField] private AnimationCurve walkSpeedOnMoveCurve;
    [SerializeField] private AnimationCurve walkSpeedOffMoveCurve;
    [Space(8)]
    [SerializeField] private float maxSpeedOnJump;
    [SerializeField] private AnimationCurve maxSpeedOnJumpCurve;
    [SerializeField] private float maxSpeedOffJumpTime;
    [SerializeField] private AnimationCurve maxSpeedOffJumpCurve;

    [Header("Air Curves")]
    [SerializeField] private float airControlMinimum;
    [SerializeField] private AnimationCurve airControlCurve;

    private ControllerPlanete playerController;
    private Rigidbody playerRb;
    private Vector3 lastVelocity;
    private Vector3 currentVelocity;

    private void Start()
    {
        playerController = player.GetComponent<ControllerPlanete>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        lastVelocity = currentVelocity;
        currentVelocity = playerRb.velocity;
    }

    #region SpeedReducOnJump
    private float maxSpeedOnJumpTimer;
    private bool maxSpeedOnJumpActive;
    public void MaxSpeedOnJumpStart()
    {
        maxSpeedOnJumpActive = true;
        maxSpeedOnJumpTimer = 0;
    }

    public float MaxSpeedOnJumpTick(float jumpTime, float baseMaxSpeed)
    {
        maxSpeedOnJumpTimer += Time.deltaTime;
        Mathf.Max(maxSpeedOnJumpTimer, jumpTime);
        float newMaxSpeed = Mathf.Lerp(baseMaxSpeed, maxSpeedOnJump, maxSpeedOnJumpCurve.Evaluate(maxSpeedOnJumpTimer / jumpTime));
        return newMaxSpeed;
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

    public float MaxSpeedOffJumpTick(float jumpTime, float baseMaxSpeed)
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
    #endregion
}
