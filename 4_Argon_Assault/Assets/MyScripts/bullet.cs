using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public GameObject explo;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
       
    }	
	
	void OnCollisionEnter(Collision col)
    {  
        if(col.collider.tag != "Player")
        {
            ContactPoint contact = col.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            GameObject.Instantiate(explo, pos, rot);
            Destroy(gameObject);
        }           	
	}
}
