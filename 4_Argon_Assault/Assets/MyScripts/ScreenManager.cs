using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Text AmmoText;
    public Text itemText;
    public Text waveText;

    PlayerShootControl shootingController;    
    [HideInInspector] public Interact amountLeft;
    [HideInInspector] public EnemySpawnManager waveCount;

    void Start()
    {        
        shootingController = FindObjectOfType<PlayerShootControl>();        
        amountLeft = FindObjectOfType<Interact>();
        waveCount = FindObjectOfType<EnemySpawnManager>();
        
        UpdateAmmoText(shootingController._maxAmmo, shootingController._maxAmmo);
        UpdateItemText(PlayerInventory.keyCount, amountLeft.keysNeeded);
        UpdateWaveText(waveCount._currentWave + 1, waveCount._totalWaves + 1);
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
    public void UpdateWaveText(float currentWave, float totalWaves)
    {
        waveText.text = "Wave " + currentWave + "/" + totalWaves;
    }
}
