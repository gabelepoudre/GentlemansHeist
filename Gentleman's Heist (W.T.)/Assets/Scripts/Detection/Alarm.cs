using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Purely in-code class. Any script should be able to interact. Do not attach to a physical alarm or
 * anything else
 */
public static class Alarm
{
    private static bool _isTriggered = false;
    private static bool _canTrigger = true;
    
    /*
     * Triggers the alarm if it's not disabled
     */
    public static void On()
    {
        if (_canTrigger)
        {
            _isTriggered = true;
        }
    }

    /*
     * Turns of the alarm regardless of if it's disabled (should never be disabled and on anyway)
     */
    public static void Off()
    {
        _isTriggered = false;
    }

    /*
     * Turns the alarm off AND makes it impossible to turn back on without a Repair()
     */
    public static void Disable()
    {
        _canTrigger = false;
        _isTriggered = false;
    }

    /*
     * Allows changes to the alarm be registered again, DOES NOT TURN ALARM ON regardless of pre-disabled state
     */
    public static void Repair()
    {
        _canTrigger = true;
    }

    /*
     * Swaps the state of the alarm if not disabled
     */
    public static void Flip()
    {
        if (_canTrigger)
        {
            _isTriggered = !_isTriggered;
        }
    }

    /*
     * Returns true if the alarm is on
     */
    public static bool IsOn()
    {
        return _isTriggered;
    }

    /*
     * Returns true if alarm is disabled
     */
    public static bool IsDisabled()
    {
        return !_canTrigger;
    }
}
