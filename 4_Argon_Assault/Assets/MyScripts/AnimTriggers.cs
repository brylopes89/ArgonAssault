using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AnimTriggers : MonoBehaviour
{
    [SerializeField] private List<Animator> anims = new List<Animator>();
    private CollisionHandler collisionTriggers;

    // Start is called before the first frame update
    void Start()
    {
        collisionTriggers = FindObjectOfType<CollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionTriggers.isWithinTrigger)
        {
            anims[0].SetBool("IsNearLandingPad", true);
            anims[1].SetBool("PlayText", true);

            if (CrossPlatformInputManager.GetButtonDown("Submit"))
            {
                anims[1].SetTrigger("KeyPress");
            }

            if (anims[0].GetBool("IsFlying") == true && anims[0].GetBool("IsLiftOff") == false)
            {
                collisionTriggers.isFlying = false;
            }
            else
            {
               collisionTriggers.isFlying = true;
            }
        }
        else if(!collisionTriggers.isWithinTrigger)
        {
            anims[0].SetBool("IsNearLandingPad", false);
            anims[1].SetBool("PlayText", false);
        }
    }
}
