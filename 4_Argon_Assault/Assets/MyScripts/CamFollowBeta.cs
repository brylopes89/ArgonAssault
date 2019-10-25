using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CamFollowBeta : MonoBehaviour
{
    public Transform target; //What the camera looks at. Generally the targeter.
    public PlayerFlightControl control; //The PlayerFlightControl script that is in play.
    Transform camTransform;
    Vector3 camPos;

    public float follow_distance = 3.0f; //How far behind the camera will follow the targeter.
    public float camera_elevation = 3.0f; //How high the camera will rise above the targeter's Z axis.
    public float follow_tightness = 5.0f; //How closely the camera will follow the target. Higher values are snappier, lower results in a more lazy follow.
    public float rotation_tightness = 10.0f; //How closely the camera will react to rotations, similar to above.    
    public float yawMultiplier = 0.005f; //Curbs the extremes of input. This should be a really small number. Might need to be tweaked, but do it as a last resort.

    [Header("Afterburner Settings")]
    public bool shake_on_afterburn = true; //The camera will shake when afterburners are active.
    public float shake_Mag = 0.7f; //How much the camera will shake when afterburners are active.                                          
    public float shakeDuration = 0f;
    public float decreaseFactor = 1f;

    public FlightAnim flightAnim;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    void Awake()
    {

        //instance = this;

    }
    private void Start()
    {
        //camTransform = gameObject.transform;
        //originalPos = camTransform.position;
    }
    void LateUpdate()
    {

        if (target == null)
        {
            Debug.LogError("(Flight Controls) Camera target is null!");
            return;
        }

        if (control == null)
        {
            Debug.LogError("(Flight Controls) Flight controller is null on camera!");
            return;
        }

        NewRotation();
    }

    void NewRotation()
    {

        //transform.position = newPosition;
        transform.position = target.position;

        Quaternion curRotation = control.actual_model.transform.rotation;

        Quaternion newRotation;

        if (control.afterburner_Active && shake_on_afterburn)
        {            
            

           // transform.localPosition = shakeTran;

            newRotation = Quaternion.Euler(transform.localPosition + new Vector3(Random.Range(-shake_Mag, shake_Mag),
            Random.Range(-shake_Mag, shake_Mag),
            Random.Range(-shake_Mag, shake_Mag)));
        }
        else
        {
           
            newRotation = curRotation;
            
        }

        transform.rotation = newRotation;
    }

  
   
}
