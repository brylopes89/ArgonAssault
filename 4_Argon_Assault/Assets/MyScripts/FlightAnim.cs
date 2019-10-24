using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAnim : MonoBehaviour
{
    public enum FlightState { None, Ground, Flight };
    private FlightState _state = FlightState.Ground;   

    public FlightState state
    {
        get { return _state; }
        set { _state = value; }
    }

    GameObject parent;
    Rigidbody player;
    public Animator anim;
    //public bool isActive;

    [Range(-1.0f, 1.0f)]
    protected float _inputGroundForward;
    [Range(-1.0f, 1.0f)]
    protected float _inputGroundTurning;

    protected bool _inputSubmit;
    public bool InputSubmit { get { return _inputSubmit; } }

    protected bool _inputTakeoff = false;
    public bool InputTakeoff { get { return _inputTakeoff; } }

    public float InputGroundForward { get { return _inputGroundForward; } }
    public float InputGroundTurning { get { return _inputGroundTurning; } }

    public bool enabledGround = true;
    public bool enabledTakeoff = true;
    //public AudioClip takeoffSoundClip;
    //private AudioSource takeoffSoundSource;

    public bool enabledLaunchIfAirborn = true;
    public float minHeightToLaunchIfAirborn = 2f;

    //meters/second
    public float maxGroundForwardSpeed = 40;
    //degrees/second
    public float groundDrag = 1;
    public float maxGroundTurningDegreesSecond = 40;

    public float speedFloat = 20.0f;
    bool isActive;

    void Awake()
    {
        //setupSound(takeoffSoundClip, ref takeoffSoundSource);
        //setupSound(landingSoundClip, ref landingSoundSource);
       
        //GetComponent<Rigidbody>().freezeRotation = true;
        //GetComponent<Rigidbody>().isKinematic = false;
    }


    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("Player");
        player = parent.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {       
        getGroundInputs();
    }   

    // Update is called once per frame
    void FixedUpdate()
    {        
         applyGroundInputs();           
    }

    void getGroundInputs()
    {
        _inputGroundForward = Input.GetAxis("Vertical");
        _inputGroundTurning = Input.GetAxis("Horizontal");
        _inputSubmit = Input.GetButton("Submit");
        _inputTakeoff = _inputSubmit;
    }

    void applyGroundInputs()
    {
        if (_inputTakeoff)
        {           
            //takeoff();
           // SendMessage("TriggerActive()");           
        }
        
        groundMove();
        //launchIfAirborn(minHeightToLaunchIfAirborn);
    }

    private void groundMove()
    {        
        player.drag = 5.0f;

        if (_inputGroundForward > 0f)
        {               
            anim.enabled = false;
            anim.SetBool("Movement", true);
            player.AddRelativeForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);            
        }
        else
        {
            anim.enabled = true;
            anim.SetBool("Movement", true);
        }

        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        player.rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);

        anim.SetFloat("ForwardSpeed", player.velocity.magnitude);
        anim.SetFloat("AngularSpeed", turningSpeed);

        anim.transform.position = player.transform.position;
       
    }

    /*private void launchIfAirborn(float minHeight)
    {
        if (!Physics.Raycast(transform.position, Vector3.down, minHeight))
        {
            takeoff();
        }
    }

    protected void takeoff()
    {
        if (!isFlying())
        {
            state = FlightState.Flight;            
           // GetComponent<Rigidbody>().isKinematic = false;
            //playSound(takeoffSoundSource);
        }   
    }*/

    /*IEnumerator TriggerActive()
    {
        anim.SetTrigger("LiftOff");
        isActive = true;
        yield return new WaitForSeconds(5.1f);
        anim.enabled = false;
        isActive = false;
    }

    /*protected AudioSource setupSound(AudioClip sound, ref AudioSource source)
    {

        if (!sound && source)
            Destroy(source);

        if (!sound && !source)
            return null;

        if (sound && !source)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
        }

        if (!source.clip)
        {
            source.clip = sound;
        }

        return source;
    }

    protected void playSound(AudioSource source)
    {
        if (source)
        {
            source.Play();
        }

    }*/

}
