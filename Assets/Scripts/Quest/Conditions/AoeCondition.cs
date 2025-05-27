using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeCondition: Condition
{
    private enum AoeCategory
    {
        Repulse,
        DoubleRepulse,
        RepulseBounce,
        RepulseImmuable,
        RepulseRocket,
        AnyRepulse,
        Magnet,
        DoubleMagnet,
        MagnetRepulse,
        MagnetBounce,
        MagnetImmuable,
        MagnetRocket,
        AnyMagnet,
    }

    [SerializeField] private AoeCategory AoeType;
    [Tooltip("Facultatif. Si l'objet doit être détruit, le lien vers le script de destruction pour l'application de la vélocité")]
    [SerializeField] private BreakableObject breakableScript;

    public BreakableObject CheckRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.Repulse || AoeType == AoeCategory.AnyRepulse)
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
    public BreakableObject CheckDoubleRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.DoubleRepulse || AoeType == AoeCategory.AnyRepulse)
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
    public BreakableObject CheckRepulseBounceAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseBounce || AoeType == AoeCategory.AnyRepulse)
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
    public BreakableObject CheckRepulseImmuableAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseImmuable || AoeType == AoeCategory.AnyRepulse)
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
    public BreakableObject CheckRepulseRocketAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseRocket || AoeType == AoeCategory.AnyRepulse)
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
    public BreakableObject CheckMagnetRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetRepulse || AoeType == AoeCategory.AnyMagnet)
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
