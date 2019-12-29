using UnityEngine;
using System;
using System.Collections;
using Papae.UnitySDK.Managers;

[Serializable]
public class PlayerFlightControl : MonoBehaviour
{
    //"Objects", "For the main ship Game Object and weapons"));
    public GameObject actual_model; //"Ship GameObject", "Point this to the Game Object that actually contains the mesh for the ship. Generally, this is the first child of the empty container object this controller is placed in."
    public Transform weapon_hardpoint_1;
    public Transform weapon_hardpoint_2;//"Weapon Hardpoint", "Transform for the barrel of the weapon"
    public GameObject bullet; //"Projectile GameObject", "Projectile that will be fired from the weapon hardpoint."   

    //"Core Movement", "Controls for the various speeds for different operations."
    public float speed = 20; //"Base Speed", "Primary flight speed, without afterburners or brakes"
    public float afterburner_speed = 40f; //Afterburner Speed", "Speed when the button for positive thrust is being held down"
    public float slow_speed = 4f; //"Brake Speed", "Speed when the button for negative thrust is being held down"    
    public float thrust_transition_speed = 5f; //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
    public float brake_transition_speed = 5f; //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
    public float turnspeed = 15.0f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
    public float rollSpeedModifier = 7; //"Roll Speed", "Multiplier for roll speed. Base roll is determined by turn speed"
    public float pitchYaw_strength = 0.5f; //"Pitch/Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"    

    //"Banking", "Visuals only--has no effect on actual movement"
    public bool use_banking = true; //Will bank during turns. Disable for first-person mode, otherwise should generally be kept on because it looks cool. Your call, though.
    public float bank_angle_clamp = 360; //"Bank Angle Clamp", "Maximum angle the spacecraft can rotate along the Z axis."
    public float bank_rotation_speed = 3f; //"Bank Rotation Speed", "Rotation speed along the Z axis when yaw is applied. Higher values will result in snappier banking."
    public float bank_rotation_multiplier = 1f; //"Bank Rotation Multiplier", "Bank amount along the Z axis when yaw is applied."

    public float screen_clamp = 500; //"Screen Clamp (Pixels)", "Once the pointer is more than this many pixels from the center, the input in that direction(s) will be treated as the maximum value."

    [HideInInspector]
    public float roll, yaw, pitch; //Inputs for roll, yaw, and pitch, taken from Unity's input system.
    [HideInInspector]
    public bool afterburner_Active = false; //True if afterburners are on.
    [HideInInspector]
    public bool slow_Active = false; //True if brakes are on
    [HideInInspector]
    public bool defeated = false;

    float distFromVertical; //Distance in pixels from the vertical center of the screen.
    float distFromHorizontal; //Distance in pixel from the horizontal center of the screen.

    Vector2 mousePos = new Vector2(0, 0); //Pointer position from CustomPointer

    float DZ = 0; //Deadzone, taken from CustomPointer.
    public float currentMag = 0f; //Current speed/magnitude       

    public bool thrust_exists = true;
    bool brake_exists = true;
    bool roll_exists = true;
    

    //public AudioClip thrust;
    AudioSource audioSource;
    AudioClip thrustClip;
    //---------------------------------------------------------------------------------

    void Start()
    {       
        mousePos = new Vector2(0, 0);
        DZ = CustomPointer.instance.deadzone_radius;
        GetSound();
        

        roll = 0; //Setting this equal to 0 here as a failsafe in case the roll axis is not set up.

        //Error handling, in case one of the inputs aren't set up.
        try
        {
            Input.GetAxis("Thrust");
        }
        catch
        {
            thrust_exists = false;
            Debug.LogError("(Flight Controls) Thrust input axis not set up! Go to Edit>Project Settings>Input to create a new axis called 'Thrust' so the ship can change speeds.");
        }

        try
        {
            Input.GetAxis("Brake");
        }
        catch
        {
            brake_exists = false;
            Debug.LogError("(Flight Controls) Brake input axis not set up! Go to Edit>Project Settings>Input to create a new axis called 'Break' so the ship can change speeds.");
        }

        try
        {
            Input.GetAxis("Roll");
        }
        catch
        {
            roll_exists = false;
            Debug.LogError("(Flight Controls) Roll input axis not set up! Go to Edit>Project Settings>Input to create a new axis called 'Roll' so the ship can roll.");
        }
    }

    public void FixedUpdate()
    {
        if (actual_model == null)
        {
            Debug.LogError("(FlightControls) Ship GameObject is null.");
            return;
        }

        currentMag = GetComponent<Rigidbody>().velocity.magnitude; //Getting the current speed.            
        
        if (!defeated)
        {
            updateCursorPosition();
            RollInput();
            ProcessAxisInput();
            RigidBodyValues();

            if (use_banking)
            {
                updateBanking(); //Calculate banking.
            }
        }    
    }

    private void RigidBodyValues()
    {
        GetComponent<Rigidbody>().AddRelativeTorque((pitch * turnspeed * Time.deltaTime), (yaw * turnspeed * Time.deltaTime), (roll * turnspeed * (rollSpeedModifier / 2) * Time.deltaTime));
        GetComponent<Rigidbody>().velocity = transform.forward * currentMag; //Apply speed
    }

    private void ProcessAxisInput()
    {
        if (thrust_exists) 
        {
            if (brake_exists && Input.GetAxis("Brake") > 0 || brake_exists && Input.GetButton("BrakeKey")) //If input on the thrust axis is negatve, activate brakes.
            {
                ApplyBrake();

            }
            else if (Input.GetAxis("Thrust") > 0 || Input.GetButton("ThrustKey"))//If input on the thrust axis is positive, activate afterburners.
            {
                BroadcastMessage("CurveIncrease", true);
                IncreaseThrust();
                audioSource.enabled = true;
               
            }

            else //Otherwise, hold normal speed.
            {
                DecreaseThrust();
               // audioSource.Stop();
                audioSource.enabled = false;
                //audioSource.loop = false;
                BroadcastMessage("CurveDecrease", true);
            }
        }
       

        //print(currentMag);
    }

    private void RollInput()
    {
        //Clamping the pitch and yaw values, and taking in the roll input.
        pitch = Mathf.Clamp(distFromVertical, -screen_clamp - DZ, screen_clamp + DZ) * pitchYaw_strength;
        yaw = Mathf.Clamp(distFromHorizontal, -screen_clamp - DZ, screen_clamp + DZ) * pitchYaw_strength;

        if (roll_exists)
            roll = (Input.GetAxis("Roll") * -rollSpeedModifier);
    }

    void IncreaseThrust()
    {
        afterburner_Active = true;       
        currentMag = Mathf.Lerp(currentMag, afterburner_speed, thrust_transition_speed * Time.deltaTime);
        
        //print(currentMag);
    }

    public void DecreaseThrust()
    {
        afterburner_Active = false;       
        currentMag = Mathf.Lerp(currentMag, speed, thrust_transition_speed * Time.deltaTime);              
    }

    void ApplyBrake()
    {        
        slow_Active = true;
        afterburner_Active = false;
        currentMag = Mathf.Lerp(currentMag, slow_speed, thrust_transition_speed * Time.deltaTime);        
    }

    void updateCursorPosition()
    {
        mousePos = CustomPointer.pointerPosition;

        //Calculate distances from the center of the screen.
        float distV = Vector2.Distance(mousePos, new Vector2(mousePos.x, Screen.height / 2));
        float distH = Vector2.Distance(mousePos, new Vector2(Screen.width / 2, mousePos.y));

        //If the distances are less than the deadzone, then we want it to default to 0 so that no movements will occur.
        if (Mathf.Abs(distV) < DZ)
            distV = 0;
        else
            distV -= DZ;
        //Subtracting the deadzone from the distance. If we didn't do this, there would be a snap as it tries to go to from 0 to the end of the deadzone, resulting in jerky movement.

        if (Mathf.Abs(distH) < DZ)
            distH = 0;
        else
            distH -= DZ;

        //Clamping distances to the screen bounds.	
        distFromVertical = Mathf.Clamp(distV, 0, (Screen.height));
        distFromHorizontal = Mathf.Clamp(distH, 0, (Screen.width));

        //If the mouse position is to the left, then we want the distance to go negative so it'll move left.
        if (mousePos.x < Screen.width / 2 && distFromHorizontal != 0)
        {
            distFromHorizontal *= -1;
        }
        //If the mouse position is above the center, then we want the distance to go negative so it'll move upwards.
        if (mousePos.y >= Screen.height / 2 && distFromVertical != 0)
        {
            distFromVertical *= -1;
        }
    }

    void updateBanking()
    {

        //Load rotation information.
        Quaternion newRotation = transform.rotation;
        Vector3 newEulerAngles = newRotation.eulerAngles;

        //Basically, we're just making it bank a little in the direction that it's turning.
        newEulerAngles.z += Mathf.Clamp((-yaw * turnspeed * Time.deltaTime) * bank_rotation_multiplier, -bank_angle_clamp, bank_angle_clamp);
        newRotation.eulerAngles = newEulerAngles;

        //Apply the rotation to the gameobject that contains the model.
        actual_model.transform.rotation = Quaternion.Slerp(actual_model.transform.rotation, newRotation, bank_rotation_speed * Time.deltaTime);

    }

    void GetSound()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = false;
        //audioSource.loop = false;
        thrustClip = audioSource.clip;
    }
}