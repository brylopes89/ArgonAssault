using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Text AmmoText;
    PlayerShootControl shootingController;

    void Start()
    {        
        shootingController = FindObjectOfType<PlayerShootControl>();
        
        UpdateAmmoText(shootingController._maxAmmo, shootingController._maxAmmo);
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
}
