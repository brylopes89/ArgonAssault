using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ParticleEmission : MonoBehaviour
{
    public GameObject[] thrusters; //thruster emission   

    List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    public GameObject throttle;
   
    float min = 0.2f;
    float max = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        throttle = GameObject.FindGameObjectWithTag("Throttle");

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

    public void CurveIncrease(bool isActive)
    {
        foreach(ParticleSystem ps in particleSystems)
        {
            var main = ps.main;

            
                      
            main.startSpeed = new ParticleSystem.MinMaxCurve(50.0f, 100.0f);                
            main.startSize = new ParticleSystem.MinMaxCurve(3.0f, 10.0f);
            main.simulationSpeed = 100;         
        }        
    }

    public void CurveDecrease(bool isActive)
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;            
           
            main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);
            main.startSize = new ParticleSystem.MinMaxCurve(2.0f, 5.0f);
            main.simulationSpeed = 1;           
        }
    }
}
