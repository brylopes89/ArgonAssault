using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        //gameObject.GetComponentInParent<EnemySpawn>().PullTrigger(other);
    }
}

