using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar_Script : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image Health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /** (An attempt at making the gradient stuff work)
        if ((float)slider.value / slider.maxValue >= 0.5)
        {
            GetComponent<SpriteRenderer>().color = new Color(2 - 2 * (float)slider.value / slider.maxValue, 1, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 2 * (float)slider.value / slider.maxValue, 0);
        }
        */
    }
    
    public void setHealth(int health)
    {
        slider.value = health;
        Health.color = gradient.Evaluate(slider.normalizedValue);
    }
    
    public void setMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        Health.color = gradient.Evaluate(slider.normalizedValue);
    }
}