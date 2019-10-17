using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ParticleEmission : MonoBehaviour
{
    public GameObject[] thrusters; //thruster emission   

    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
   
    float min = 0.2f;
    float max = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in thrusters)
        {
            var ps = obj.GetComponent<ParticleSystem>();
            particleSystems.Add(ps);            
        }
    }
    // Update is called once per frame
    void Update()
    {
       // CurveIncrease(true);
    }   

    void CurveIncrease(bool isActive)
    {
        foreach(ParticleSystem ps in particleSystems)
        {
            var main = ps.main;

            if (Input.GetAxis("Thrust")>0)
            {                
                main.startSpeed = new ParticleSystem.MinMaxCurve(50.0f, 100.0f);                
                main.startSize = new ParticleSystem.MinMaxCurve(3.0f, 10.0f);
                main.simulationSpeed = 100;
            }
            else
            {
                main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);                
                main.startSize = new ParticleSystem.MinMaxCurve(2.0f, 5.0f);
                main.simulationSpeed = 1;
            }
        }        
    }
}
