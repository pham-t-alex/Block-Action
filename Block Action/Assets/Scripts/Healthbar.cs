using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour
{
    Slider _slider;
    public Slider slider
    {
        get
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }
            return _slider;
        }
    }
    public Image Health;

    static GameObject _healthCanvas;
    public static GameObject healthCanvas {
        get
        {
            if (_healthCanvas == null)
            {
                _healthCanvas = GameObject.FindWithTag("HealthbarsAndEffects");
            }
            return _healthCanvas;
        }
    }

    private GameObject _healthNumber;
    public GameObject healthNumber
    {
        get
        {
            if (_healthNumber == null)
            {
                _healthNumber = transform.GetChild(2).gameObject;
            }
            return _healthNumber;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)slider.value / slider.maxValue >= 0.5)
        {
            Health.color = new Color(2 - 2 * (float)slider.value / slider.maxValue, 1, 0);
        }
        else
        {
            Health.color = new Color(1, 2 * (float)slider.value / slider.maxValue, 0);
        }
    }
    
    public void setHealth(int health, int maxHealth)
    {
        slider.value = 100 * health / maxHealth;
        healthNumber.GetComponent<TMP_Text>().text = health + "";
    }
}