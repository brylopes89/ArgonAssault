using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("FX pefab on player")] [SerializeField] GameObject deathFX;
    public Slider HealthBar;   
    public float Health = 120f;       

    private float _maxHealth;
    private Image _targetBar;   
    private GameManager _gameManager;
    

    // Start is called before the first frame update
    void Start()
    {       
        _gameManager = FindObjectOfType<GameManager>();

        HealthBar.maxValue = Health;
        _maxHealth = Health;
        if (HealthBar.fillRect != null)
            _targetBar = HealthBar.fillRect.GetComponent<Image>();

        MatchAmount();
        MatchHPbarColor();
    }

    private void Update()
    {
        MatchAmount();
        MatchHPbarColor();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (Health > 0)
        {
            Health -= GameObject.FindObjectOfType<Enemy>().EnemyWeaponDamage();
            HealthBar.value = Health;
        }

        if (Health <= 0)
        {                            
            StartDeathSequence();    
        }    
    }

    public void StartDeathSequence()
    {
        Health = 0;
        print("Wipe yourself off, you dead.");
        GameObject fx = Instantiate(deathFX, transform.position, Quaternion.identity);
        _gameManager.GameOver();
        //Invoke("ReloadScene", levelLoadDelay);
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
        HealthBar.value = Health;
        _targetBar.fillAmount = Health / _maxHealth;
    }
}
