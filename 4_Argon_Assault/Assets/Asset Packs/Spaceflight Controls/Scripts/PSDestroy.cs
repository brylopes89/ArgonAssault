using UnityEngine;
using System.Collections;

public class PSDestroy : MonoBehaviour {

    //private ParticleSystem ps;

    void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
