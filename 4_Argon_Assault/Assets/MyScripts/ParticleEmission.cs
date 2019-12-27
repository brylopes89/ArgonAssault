using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ParticleEmission : MonoBehaviour
{
    public GameObject[] thrusters; //thruster emission   
    public Slider sliderValue;

    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    ParticleSystem.EmissionModule emissionModule;
    
    float min = 0.2f;
    float max = 5.0f;
    float maxEm = 79f;
    float minEm = 50f;
    float sliderMultiplier = 50.0f;   
    
    // Start is called before the first frame update
    void Start()
    {
        sliderValue = FindObjectOfType<Slider>();    
        
        foreach(GameObject obj in thrusters)
        {
            var ps = obj.GetComponent<ParticleSystem>();            
            particleSystems.Add(ps);            
        }
    }

    public void CurveIncrease(bool isActive)
    {
        foreach(ParticleSystem ps in particleSystems)
        {
            var main = ps.main;            

            emissionModule = ps.emission;
            emissionModule.rateOverTime = AdditionalParticleValue() + minEm;          
            main.startSpeed = new ParticleSystem.MinMaxCurve(AdditionalParticleValue() + min, AdditionalParticleValue() + max);                
            //main.startSize = new ParticleSystem.MinMaxCurve(3.0f, 10.0f);
            main.simulationSpeed = AdditionalParticleValue();                     
        }        
    }

    public void CurveDecrease(bool isActive)
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;            
            emissionModule = ps.emission;
            emissionModule.rateOverTime = minEm;            
            main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);
            main.startSize = new ParticleSystem.MinMaxCurve(2.0f, 5.0f);
            main.simulationSpeed = 1;           
        }
    }

    float AdditionalParticleValue()
    {
        if (sliderValue.value >= 0f)
        {
            return sliderValue.value * sliderMultiplier;
        }

        else 
        {
            return 0;
        }        
    }
}
