using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    // Sets maxHealth on UI Slider component and sets current/slider value to max
    public void setMaxHealth(int healthMax)
    {
      slider.maxValue = healthMax;
      slider.value = healthMax;
      fill.color = gradient.Evaluate(1f);
      
    }

    // Sets current value on UI slider component value to given value
    public void setHealth(int health)
    {
      slider.value = health;
      fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
