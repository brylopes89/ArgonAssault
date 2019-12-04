using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider HealthBar;
    private float Health = 120f;
    private float _maxHealth;
    private Image _targetBar;

    bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar.maxValue = Health;
        _maxHealth = Health;
        if (HealthBar.fillRect != null)
            _targetBar = HealthBar.fillRect.GetComponent<Image>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        isHit = true;
        Health -= GameObject.FindObjectOfType<Enemy>().EnemyWeaponDamage();
    }
    public void GetHit(float hitAmount)
    {
        
        Health -= hitAmount;
        if (Health <= 0)
        {
            Health = 0;
            HealthBar.value = Health;
            MatchHPbarColor();
        }            
    }
    void MatchHPbarColor()
    {
        var currentHealthPercentage = (Health * 100) / _maxHealth;
        if (currentHealthPercentage >= 75)
        {
            _targetBar.color = Color.green;
        }
        else if (currentHealthPercentage < 75 && currentHealthPercentage >= 25)
        {
            _targetBar.color = Color.yellow;
        }
        else if (currentHealthPercentage < 25)
        {
            _targetBar.color = Color.red;
        }
    }

    void MatchAmount()
    {
        _targetBar.fillAmount = Health / _maxHealth;
    }
}
