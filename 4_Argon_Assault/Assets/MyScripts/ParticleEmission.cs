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
    ParticleSystem.MainModule main;


    float min = 0.2f;
    float max = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        thrusters = new GameObject[ps];

        ps = GameObject.FindObjectOfType("
        em = new ParticleSystem.MinMaxCurve[thrusters.Length];
        for (int i = 0; i < thrusters.Length; i++)
        {
            em[i] = thrusters[i].GetComponentsInChildren<GameObject>();
        }
        //ps = GetComponentsInChildren<ParticleSystem>()[thrusters.Length];
       // ps = GetComponents<GameObject>ParticleSystem
        main = ps.main;

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
            foreach (GameObject thruster in thrusters)
            {
                main.startSpeed = new ParticleSystem.MinMaxCurve(50.0f, 100.0f);
                main.simulationSpeed = 100;
            }
        }
        
    } 
}
