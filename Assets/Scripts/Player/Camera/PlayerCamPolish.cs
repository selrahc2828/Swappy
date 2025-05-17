using UnityEngine;

    public class PlayerCamPolish : MonoBehaviour
    {
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


        private Vector3 mergedCamOffset;
    }
