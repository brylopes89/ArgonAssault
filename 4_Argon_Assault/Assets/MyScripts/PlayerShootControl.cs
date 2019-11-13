using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootControl : MonoBehaviour
{
    [Header("Weapon Damage")]
    public int weaponDamage = 10;

    public List<GameObject> firePoint = new List<GameObject>();
    public List<GameObject> vfx = new List<GameObject>();
    public List<GameObject> _cameras = new List<GameObject>();

    private GameObject vfx1;
    private GameObject vfx2;
    private Transform _player;

    private GameObject effectToSpawn;    
    private AudioSource _audioSource;

    public AudioClip MissileLaunchSfx;
    public float Range = 100;
    public float MaxAmmo = 10f;
    public float ShootingDelay = 0.1f;   

    private float _currentAmmo;
    private float _timer;

    private bool _isShooting;
    private bool _isReloading;
    private LineRenderer _lineRenderer;

    Ray vRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {        
        effectToSpawn = vfx[0];
        _currentAmmo = MaxAmmo;
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        SetupSound();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;        

        if (Input.GetButtonDown("Fire1") && _timer >= ShootingDelay && !_isReloading)
        {
            Fire();
        }
        /*else if (!Input.GetButtonDown("Fire1"))
        {
            _audioSource.Stop();
        }*/
      
        if (Input.GetButtonDown("Reload"))
        {
            StartReloading();
        }
    }

    void StartReloading()
    {       
        _isShooting = false;
        _isReloading = true;
    }

    void Fire()
    {       
        if (firePoint != null)
        {
            vfx1 = Instantiate(effectToSpawn, firePoint[0].transform.position, Quaternion.identity);
            vfx2 = Instantiate(effectToSpawn, firePoint[1].transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No Fire Point");
        }

        _timer = 0;     
        _audioSource.Play();


        Debug.DrawLine(_player.position, hit.point, Color.green);

        // if (!CustomPointer.instance.center_lock)
        vRay = Camera.main.ScreenPointToRay(CustomPointer.pointerPosition);
       // else
           // vRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(vRay, out hit))
        {           
            print("hit " + hit.collider.gameObject);
            //vfx1.transform.LookAt(vRay.direction);
            //vfx2.transform.LookAt(vRay.direction);   
            Debug.DrawLine(_player.position, hit.point, Color.green);
        }
        else
        {
            Vector3 direction = (vRay.GetPoint(100000.0f) - vfx1.transform.position).normalized;
            vfx1.transform.LookAt(vRay.direction);
            vfx2.transform.LookAt(vRay.direction);
            
        }       
    }  

    public int CalculateWeaponDamage()
    {
        int damageDealt = weaponDamage;
        return damageDealt;
    }

    void SetupSound()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.volume = 1.0f;
        _audioSource.clip = MissileLaunchSfx;
    }
}
