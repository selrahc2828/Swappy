using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeCondition: Condition
{
    private enum AoeCategory
    {
        Impulse,
        DoubleImpulse,
        ImpulseBounce,
        ImpulseImmuable,
        ImpulseRocket,
        AnyImpulse,
        Magnet,
        DoubleMagnet,
        MagnetImpulse,
        MagnetBounce,
        MagnetImmuable,
        MagnetRocket,
        AnyMagnet,
    }

    [SerializeField] private AoeCategory AoeType;
    [Tooltip("Facultatif. Si l'objet doit être détruit, le lien vers le script de destruction pour l'application de la vélocité")]
    [SerializeField] private BreakableObject breakableScript;

    public BreakableObject CheckImpulseAoeCondition()
    {
        if (AoeType == AoeCategory.Impulse || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckDoubleImpulseAoeCondition()
    {
        if (AoeType == AoeCategory.DoubleImpulse || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckImpulseBounceAoeCondition()
    {
        if (AoeType == AoeCategory.ImpulseBounce || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckImpulseImmuableAoeCondition()
    {
        if (AoeType == AoeCategory.ImpulseImmuable || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckImpulseRocketAoeCondition()
    {
        if (AoeType == AoeCategory.ImpulseRocket || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckMagnetAoeCondition()
    {
        if (AoeType == AoeCategory.Magnet || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckDoubleMagnetAoeCondition()
    {
        if (AoeType == AoeCategory.DoubleMagnet || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckMagnetImpulseAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetImpulse || AoeType == AoeCategory.AnyImpulse)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckMagnetBounceAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetBounce || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckMagnetImmuableAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetImmuable || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
    public BreakableObject CheckMagnetRocketAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetRocket || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
            return breakableScript;
        }
        else
        {
            SetConditionState(false);
            return null;
        }
    }
}
