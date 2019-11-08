using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public List<GameObject> firePoint = new List<GameObject>();
    public List<GameObject> vfx = new List<GameObject>();

    private GameObject effectToSpawn;
    GameObject vfx1;
    GameObject vfx2;

    Ray vRay;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnVFX();
        }       
    }

    void SpawnVFX()
    {       

        if (firePoint != null)
        {
            vfx1 = Instantiate(effectToSpawn, firePoint[0].transform.position, Quaternion.identity);
            vfx2 = Instantiate(effectToSpawn, firePoint[1].transform.position, Quaternion.identity);
        }

        else
        {
            Debug.Log("No Fire Point");
        }

        if (!CustomPointer.instance.center_lock)
            vRay = Camera.main.ScreenPointToRay(CustomPointer.pointerPosition);
        else
            vRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        if (Physics.Raycast(vRay, out hit))
        {
            vfx1.transform.LookAt(hit.point);
            //shot1.GetComponent<Rigidbody>().AddForce((shot1.transform.forward) * 9000f);            
            //shot2.GetComponent<Rigidbody>().AddForce((shot2.transform.forward) * 9000f);
        }
    }
}
