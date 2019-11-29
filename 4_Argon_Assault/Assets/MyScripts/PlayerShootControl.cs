using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootControl : MonoBehaviour
{
    [Header("Weapon Damage")]
    public int weaponDamage = 10;
    public float speed = 50f;
    public float _maxAmmo = 10f;
    public float _maximumLength;
    public float _switchDelay = 1.0f;
    public float _impactForce = 30.0f;
    public float _fireRate = 15.0f;

    private int _index = 0;
    private float _currentAmmo;
    private float _nextTimeToFire = 0f;

    public List<GameObject> firePoint = new List<GameObject>();
    public List<GameObject> weapons = new List<GameObject>();   
    public GameObject targeter;

    private GameObject vfx1;
    private GameObject vfx2;
    private Transform _player;     

    private bool _isSwitching;
    private bool _isShooting;
    private bool _isReloading;   

    Ray vRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {         
        _currentAmmo = _maxAmmo;
        _player = GameObject.FindGameObjectWithTag("Player").transform;        
        InitializeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && !_isSwitching)
        {
            _index++;
            if(_index >= weapons.Count - 1)
            {
                _index = 0;
            }
            StartCoroutine(SwitchAfterDelay(_index));            
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !_isSwitching)
        {
            _index--;

            if (_index < 0)
            {
                _index = weapons.Count - 1;
            }
            StartCoroutine(SwitchAfterDelay(_index));
        }

        if (Input.GetButtonDown("WeaponChange") && !_isSwitching)
        {
            _index++;
            if (_index > weapons.Count -1)
            {
                _index = 0;
            }
            StartCoroutine(SwitchAfterDelay(_index));
        }

        if (Input.GetButtonDown("Fire1") && !_isReloading && Time.time >= _nextTimeToFire && weapons[0])
        {
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Fire();
        }    
        
        if (Input.GetButton("Fire1") && !_isReloading && weapons[1])
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
                if(_index != 1)
                {
                    vfx1.GetComponent<Rigidbody>().AddForce((vfx1.transform.forward) * 9000f);
                }
                Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);                
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

                //Debug.DrawRay(firePoint[i].transform.position, firePoint[i].transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
            }
        }             
    } 

    public int CalculateWeaponDamage()
    {
        int damageDealt = weaponDamage;
        return damageDealt;
    }
}
