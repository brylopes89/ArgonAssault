using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class LookAtPoint : MonoBehaviour
{
    public Vector3 lookAtPoint = Vector3.zero;

     // Update is called once per frame
    public void Update()
    {
        transform.LookAt(lookAtPoint);
    }
}
