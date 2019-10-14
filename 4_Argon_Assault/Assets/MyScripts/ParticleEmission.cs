using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleEmission : MonoBehaviour
{
    public GameObject[] thrusters; //thruster emission   
   


    ParticleSystem ps;
    ParticleSystem.MinMaxCurve[] em;
<<<<<<< HEAD
    ParticleSystem.MainModule main;

=======
    
>>>>>>> parent of 17164d9... Changed collision detection on core ship.

    float min = 0.2f;
    float max = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        ps = gameObject.GetComponentInChildren<ParticleSystem>();
        //ps = GetComponentsInChildren<ParticleSystem>()[thrusters.Length];
       // ps = GetComponents<GameObject>ParticleSystem
        main = ps.main;

<<<<<<< HEAD
=======
        foreach(GameObject obj in thrusters)
        {
            var ps = obj.GetComponent<ParticleSystem>();
            particleSystems.Add(ps);     
        }
>>>>>>> parent of 17164d9... Changed collision detection on core ship.
    }

   

    // Update is called once per frame
    void Update()
    {

        main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);
        main.simulationSpeed = 1;
        CurveIncrease();
      
    }

    void CurveIncrease()
    {
        if (Input.GetAxis("Thrust") > 0)
        {
<<<<<<< HEAD
            foreach (GameObject thruster in thrusters)
            {
                main.startSpeed = new ParticleSystem.MinMaxCurve(50.0f, 100.0f);
                main.simulationSpeed = 100;
            }
        }
        
=======
            var main = ps.main;            

            if (Input.GetAxis("Thrust") > 0)
            {               
                main.startSpeed = new ParticleSystem.MinMaxCurve(50.0f, 100.0f);
                main.simulationSpeed = 100;
            }
            else
            {
                main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);
                main.simulationSpeed = 1;
            }
        }      
              
>>>>>>> parent of 17164d9... Changed collision detection on core ship.
    } 
}
