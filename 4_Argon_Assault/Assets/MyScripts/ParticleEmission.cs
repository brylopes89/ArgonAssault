﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ParticleEmission : MonoBehaviour
{
    public GameObject[] thrusters; //thruster emission   

    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    ParticleSystem.MinMaxCurve[] em;


    float min = 0.2f;
    float max = 5.0f;

    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject obj in thrusters)
        {
            var ps = obj.GetComponent<ParticleSystem>();
            particleSystems.Add(ps);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CurveIncrease();
    }

    void CurveIncrease()
    {
        foreach (ParticleSystem ps in particleSystems) // 
        {
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

    }
}
