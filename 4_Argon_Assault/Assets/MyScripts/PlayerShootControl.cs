using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerShootControl : MonoBehaviour
{
    [Header("Weapon Damage")]
    public int weaponDamage;
    public float speed = 50f;
    public float _maxAmmo = 10f;   
    public float _switchDelay = 1.0f;
    public float _impactForce = 30.0f;
    public float _currentAmmo;
    public GameObject slider;

    private float _fireRate;
    private float _nextTimeToFire = 0f;

    [HideInInspector] public int _index = 0;
    [HideInInspector] public bool _isReloading;

    public List<GameObject> firePoint = new List<GameObject>();
    public List<GameObject> weapons = new List<GameObject>();   
    public GameObject targeter;

    private GameObject vfx1;
    private GameObject vfx2;
    private Transform _player;
    private ScreenManager _screenManager;

    private bool _isSwitching;
    private bool _isShooting;    

    Ray vRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {         
        _currentAmmo = _maxAmmo;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _screenManager = GameObject.FindWithTag("ScreenManager").GetComponent<ScreenManager>();
        InitializeWeapons();
        _isShooting = false;
        _isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {  
        if(CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") > 0 && !_isSwitching)
        {
            _index++;
            if(_index >= weapons.Count - 1)
            {
                _index = 0;
            }
            StartCoroutine(SwitchAfterDelay(_index));            
        }

        if (CrossPlatformInputManager.GetAxis("Mouse ScrollWheel") < 0 && !_isSwitching)
        {
            _index--;

            if (_index < 0)
            {
                _index = weapons.Count - 1;
            }
            StartCoroutine(SwitchAfterDelay(_index));
        }

        if (CrossPlatformInputManager.GetButtonDown("WeaponChange") && !_isSwitching)
        {
            _index++;
            if (_index > weapons.Count -1)
            {
                _index = 0;
            }
            StartCoroutine(SwitchAfterDelay(_index));            
        }       

        if (CrossPlatformInputManager.GetButton("Fire1") && !_isReloading && Time.time >= _nextTimeToFire)
        {            
            if (_index == 0)
            {
                _fireRate = 4f;
                Fire();
            }
            else if(_index == 1 && _currentAmmo > 0)
            {
                _fireRate = 1f;
                _currentAmmo--;
                _screenManager.UpdateAmmoText(_currentAmmo, _maxAmmo);
                Fire();
            }
            _nextTimeToFire = Time.time + 1f / _fireRate;          
        }

        if (CrossPlatformInputManager.GetButtonDown("Reload") || Input.GetKeyDown(KeyCode.R))
        {
            if (_index == 1)
                StartCoroutine(StartReloading());
        }
    }

    IEnumerator StartReloading()
    {
      
        _isReloading = true;
        _isShooting = false;
        slider.SetActive(true);              

        yield return new WaitForSeconds(4f);

        _isReloading = false;
        _currentAmmo = _maxAmmo;
        _screenManager.UpdateAmmoText(_currentAmmo, _maxAmmo);
        slider.SetActive(false);

    }

    private IEnumerator SwitchAfterDelay(int newIndex)
    {
        _isSwitching = true;

        yield return new WaitForSeconds(_switchDelay);

        _isSwitching = false;
        ChangeWeapon(newIndex);
    }

    void InitializeWeapons()
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

    void Fire()
    {
        int layerMask = 1 << 10;
        layerMask = ~layerMask;

        _isShooting = true;       

        for (int i = 0; i < firePoint.Count; i++)
        {
            if (firePoint != null)
            {           
                vfx1 = Instantiate(weapons[_index], firePoint[i].transform.position, Quaternion.identity);
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
                //if(_index != 1)
                //{
                    vfx1.GetComponentInChildren<Rigidbody>().AddForce((vfx1.transform.forward) * 9000f);
                //}
                //Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);                
            }
            else
            {
                if (_index == 1)
                {
                    vfx1.transform.LookAt(targeter.transform.position);                   
                }
                else
                {
                    vfx1.GetComponentInChildren<Rigidbody>().AddForce((vRay.direction) * 9000f);
                }            
            }
            //Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * 1000, Color.white);

            if(hit.rigidbody != null)
            {
                if (_index == 0)
                    hit.rigidbody.AddForce(-hit.normal * _impactForce);
            }
        }             
    } 

    public int CalculateWeaponDamage()
    {        
        int damageDealt = weaponDamage;
        if (_index == 0)
            weaponDamage = 5;
        else
            weaponDamage = 10;
        return damageDealt;
    }
}
