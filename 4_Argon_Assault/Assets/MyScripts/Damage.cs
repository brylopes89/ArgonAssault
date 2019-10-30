using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Weapon Damage")]
    [SerializeField] int weaponDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int CalculateWeaponDamage()
    {
        int damageDealt = weaponDamage;
        return damageDealt;
    }
}
