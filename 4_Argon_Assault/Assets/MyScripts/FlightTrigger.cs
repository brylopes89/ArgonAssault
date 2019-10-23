using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightTrigger : MonoBehaviour
{
    public Animator anim;

    bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            StartCoroutine(TriggerActive());
        }
    }

    IEnumerator TriggerActive()
    {
        anim.SetTrigger("LiftOff");
        isActive = true;
        yield return new WaitForSeconds(5.1f);
        anim.enabled = false;
        isActive = false;
    }
}
