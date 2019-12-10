using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Text AmmoText;
    public Text itemText;

    PlayerShootControl shootingController;    
    [HideInInspector] public Interact amountLeft;    

    void Start()
    {        
        shootingController = FindObjectOfType<PlayerShootControl>();        
        amountLeft = FindObjectOfType<Interact>();
        
        UpdateAmmoText(shootingController._maxAmmo, shootingController._maxAmmo);
        UpdateItemText(PlayerInventory.keyCount, amountLeft.keysNeeded);
    }

    void Update()
    {
        if (shootingController._index == 0)
            AmmoText.text = "∞/∞";       
        else
            UpdateAmmoText(shootingController._currentAmmo, shootingController._maxAmmo);
    }
    public void UpdateAmmoText(float currentAmmo, float maxAmmo)
    {
        AmmoText.text = currentAmmo + "/" + maxAmmo;              
    }
    public void UpdateItemText(float currentKeys, float maxKeys)
    {
        itemText.text = currentKeys + "/" + maxKeys;
    }
}
