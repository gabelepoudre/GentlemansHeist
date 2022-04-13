using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuardData
{
    private static float guardSpeed = 200f;
    private static int guardHealth = 25;

    public static float getGuardSpeed()
    {
        return guardSpeed;
    }

    public static void setGuardSpeed(float newSpeed)
    {
        guardSpeed = newSpeed;
    }

    public static int getGuardHealth()
    {
        return guardHealth;
    }

    public static void setGuardHealth(int newHealth)
    {
        guardHealth = newHealth;
    }
}
