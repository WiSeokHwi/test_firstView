using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;
    
    
    public Slider playerHealthSlider;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        
    }

    public void UpdatePlayerHealthSlider(float currentHealth, float maxHealth)
    {
        playerHealthSlider.value = currentHealth/maxHealth;
    }
}
