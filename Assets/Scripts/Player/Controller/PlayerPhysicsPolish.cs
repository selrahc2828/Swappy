using UnityEngine;


    public class PlayerPhysicsPolish : MonoBehaviour
    {
        [Header("Physics Curves")]
        private AnimationCurve walkSpeedOnMoveCurve;
        private AnimationCurve walkSpeedOffMoveCurve;

        private float maxSpeedOnJump;
        private AnimationCurve maxSpeedOnJumpCurve;
        private float maxSpeedOffJumpTime;
        private AnimationCurve maxSpeedOffJumpCurve;

        private float airControlMinimum;
        private AnimationCurve airControlCurve;
    }
