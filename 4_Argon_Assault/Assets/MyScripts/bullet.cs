using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public GameObject explo;

    public float speed;
    public float fireRate;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        else
        {
            Debug.Log("No Speed");
        }
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
