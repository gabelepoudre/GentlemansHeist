using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyAdjustment : MonoBehaviour
{
    public void changeDifficulty()
    {
        if (PlayerData.GetKeys() == 0) //Difficulty decrease
        {
            if (GuardData.getGuardHealth() >= 20)
            {
                GuardData.setGuardHealth(GuardData.getGuardHealth() - 5);
                GuardData.setGuardSpeed(GuardData.getGuardSpeed() - 25);
            }
        }
        else if (PlayerData.GetKeys() == 1) //Small Difficulty increase
        {
            if (GuardData.getGuardHealth() <= 45)
            {
                GuardData.setGuardHealth(GuardData.getGuardHealth() + 5);
                GuardData.setGuardSpeed(GuardData.getGuardSpeed() + 25);
            }
        }
        else //Bigger Difficulty increase
        {
            if (GuardData.getGuardHealth() <= 40)
            {
                GuardData.setGuardHealth(GuardData.getGuardHealth() + 10);
                GuardData.setGuardSpeed(GuardData.getGuardSpeed() + 25);
            }
        }
    }
}
