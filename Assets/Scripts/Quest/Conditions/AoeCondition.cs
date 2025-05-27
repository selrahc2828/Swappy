using System.Collections;
using UnityEngine;

public class AoeCondition: Condition
{
    [HideInInspector] private enum AoeCategory
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

    private AoeCategory AoeType;

    public void CheckRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.Repulse || AoeType == AoeCategory.AnyRepulse)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckDoubleRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.DoubleRepulse || AoeType == AoeCategory.AnyRepulse)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckRepulseBounceAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseBounce || AoeType == AoeCategory.AnyRepulse)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckRepulseImmuableAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseImmuable || AoeType == AoeCategory.AnyRepulse)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckRepulseRocketAoeCondition()
    {
        if (AoeType == AoeCategory.RepulseRocket || AoeType == AoeCategory.AnyRepulse)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckMagnetAoeCondition()
    {
        if (AoeType == AoeCategory.Magnet || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckDoubleMagnetAoeCondition()
    {
        if (AoeType == AoeCategory.DoubleMagnet || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckMagnetRepulseAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetRepulse || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckMagnetBounceAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetBounce || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckMagnetImmuableAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetImmuable || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
    public void CheckMagnetRocketAoeCondition()
    {
        if (AoeType == AoeCategory.MagnetRocket || AoeType == AoeCategory.AnyMagnet)
        {
            SetConditionState(true);
        }
        else
        {
            SetConditionState(false);
        }
    }
}
