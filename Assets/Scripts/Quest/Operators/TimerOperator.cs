using System.Collections;
using UnityEngine;

public class TimerOperator : OperatorCondition
{
    public enum TimerTypes
    {
        Wait,
        Invert
    }

    public enum ValidationTypes
    {
        Free,
        Discrete,
        Strict
    }

    [SerializeField] private TimerTypes timerType;
    [SerializeField] private ValidationTypes validationType;
    [SerializeField] private float outputTimer;

    private bool isTimerRunning;
    private bool timerState;
    private bool currentState;
    private float timer;

    public override void OperatorInput(bool state, Condition source)
    {
        currentState = state;

        if (currentState != timerState && validationType == ValidationTypes.Strict)
        {
            ResetTimer();
            return;
        }
        
        timerState = currentState;

        if (timerType == TimerTypes.Invert)
        {
            SetConditionState(true);
        }

        isTimerRunning = true;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            if (timer >= outputTimer)
            {
                TryValidateTimer();
                ResetTimer();
            }
        }
    }

    private void TryValidateTimer()
    {
        switch (validationType)
        {
            case ValidationTypes.Free:
                ValidateTimer();
                return;
                    
            case ValidationTypes.Discrete:
                if (timerState != currentState)
                {
                    return;
                }
                ValidateTimer();
                return;

            case ValidationTypes.Strict:
                if (timerState != currentState)
                {
                    return;
                }
                ValidateTimer();
                return;
        }
    }

    private void ValidateTimer()
    {
       switch (timerType)
        {
            case TimerTypes.Invert:
                SetConditionState(false);
                return;
            case TimerTypes.Wait: 
                SetConditionState(true); 
                return;
        }
    }

    private void ResetTimer()
    {
        isTimerRunning = false;
        timer = 0;
    }
}