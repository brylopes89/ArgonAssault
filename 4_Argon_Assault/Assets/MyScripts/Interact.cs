using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //now gives you access to the function of level load
using UnityEngine.EventSystems;

public class Interact : MonoBehaviour {

    public float rayLength;
    public Material highlightMaterial;
    public LayerMask layerMask;  
    public int keysNeeded;

    private GameObject curObj;
    private RaycastHit objectHit;
    private Material savedMaterial;
    private GameOverManager restartGame;

    Ray vRay;

    private void Start()
    {
        restartGame = FindObjectOfType<GameOverManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.blue); //sets the raycast

        if (!CustomPointer.instance.center_lock)
            vRay = Camera.main.ScreenPointToRay(CustomPointer.pointerPosition);
        else
            vRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

        // If we hit something...
        if (Physics.Raycast(vRay, out objectHit, rayLength, layerMask))
        {
            if (Input.GetButtonDown("Submit") && !EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log(objectHit.transform.name);
            }            

            if (curObj == null) // == is a check. IF we hit something, and curObj is not assigned a gameobject, then...
            {
                // Assign curObj the gameObject I'm hitting
                curObj = objectHit.collider.gameObject;

                savedMaterial = curObj.GetComponent<Renderer>().material;

                curObj.GetComponent<Renderer>().material = highlightMaterial; //assigns curObj to material (the white material highlight)
                
            }

            if (curObj != null && curObj != objectHit.transform.gameObject) // != not equal to. This is staying if the game object your currently looking at is not the same as the original object, then nullify it.
            {
                NullifyCurObj();
            }
        }

        else
        {
            //ELSE, if my ray is NOT hitting something BUT curOBJ is still assigned...
            if (curObj != null)
            {
                NullifyCurObj();
            }
        }    
    }

    void NullifyCurObj()
    {
            curObj.GetComponent<Renderer>().material = savedMaterial;

            curObj = null;
    }

    void ObjInteraction(GameObject objFromRaycast)
    {
        if (objFromRaycast.tag == "Target")
        {
            PlayerInventory.keyCount++;

            print("I have " + PlayerInventory.keyCount + " keys!");

            Destroy(objFromRaycast);

        } 

        if (objFromRaycast.tag == "Door")
        {
           // if (PlayerInventory.keyCount >= keysNeeded)
           // {
            //    PlayerInventory.keyCount = 0;
            SceneManager.LoadScene("Flooded_Grounds");
        }

        else
        {
            print("I still need " + (keysNeeded - PlayerInventory.keyCount) + " keys!");
        }     
    }

}
