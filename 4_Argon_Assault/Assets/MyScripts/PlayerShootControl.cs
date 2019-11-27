using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootControl : MonoBehaviour
{
    [Header("Weapon Damage")]
    public int weaponDamage = 10;

    public List<GameObject> firePoint = new List<GameObject>();
    public List<GameObject> weapons = new List<GameObject>();
    public List<GameObject> _cameras = new List<GameObject>();
    public GameObject targeter;

    private GameObject vfx1;
    private GameObject vfx2;
    private Transform _player;
    private GameObject effectToSpawn;    
    
    public float MaxAmmo = 10f;   
    public float MaximumLength;
    public float _switchDelay = 1.0f;      

    private int index = 0;
    private float _currentAmmo;

    private bool _isSwitching;
    private bool _isShooting;
    private bool _isReloading;
    private LineRenderer _lineRenderer;    

    Ray vRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {        
        //effectToSpawn = weapons[0];
        _currentAmmo = MaxAmmo;
        _player = GameObject.FindGameObjectWithTag("Player").transform;        
        InitializeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && !_isSwitching)
        {
            index++;
            if(index >= weapons.Count - 1)
            {
                index = 0;
            }
            StartCoroutine(SwitchAfterDelay(index));            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !_isSwitching)
        {
            index--;

            if (index < 0)
            {
                index = weapons.Count - 1;
            }
            StartCoroutine(SwitchAfterDelay(index));
        }

        if (Input.GetButtonDown("WeaponChange") && !_isSwitching)
        {
            index++;
            if (index > weapons.Count -1)
            {
                index = 0;
            }
            StartCoroutine(SwitchAfterDelay(index));
        }

        if (Input.GetButtonDown("Fire1") && !_isReloading)
        {
            Fire();
        }       
      
        if (Input.GetButtonDown("Reload"))
        {
            StartReloading();
        }
    }

    private IEnumerator SwitchAfterDelay(int newIndex)
    {
        _isSwitching = true;

        yield return new WaitForSeconds(_switchDelay);

        _isSwitching = false;
        ChangeWeapon(newIndex);
    }

    private void InitializeWeapons()
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    } 

    public void ChangeWeapon(int newIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[newIndex].SetActive(true);
    }   

    void StartReloading()
    {       
        _isShooting = false;
        _isReloading = true;
    }

    void Fire()
    {
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        _isShooting = true;

        for (int i = 0; i < firePoint.Count; i++)
        {
            if (firePoint != null)
            {           
                vfx1 = Instantiate(weapons[index], firePoint[i].transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No Fire Point");
            }

            if (!CustomPointer.instance.center_lock)
                vRay = Camera.main.ScreenPointToRay(CustomPointer.pointerPosition);
            else
                vRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

            if (Physics.Raycast(vRay, out hit, layerMask))
            {
                vfx1.transform.LookAt(hit.point);
                Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);                
            }
            else
            {                             
                vfx1.transform.LookAt(targeter.transform.position);
                Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }
        }             
    } 

    public int CalculateWeaponDamage()
    {
        int damageDealt = weaponDamage;
        return damageDealt;
    }
}
