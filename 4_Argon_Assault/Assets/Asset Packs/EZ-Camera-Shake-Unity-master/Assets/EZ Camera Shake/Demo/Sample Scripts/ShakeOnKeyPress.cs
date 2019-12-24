using UnityEngine;
using EZCameraShake;
using UnityStandardAssets.CrossPlatformInput;

public class ShakeOnKeyPress : MonoBehaviour
{
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;

    void Update()
    {
        ThrustShake();
    }

    public void ThrustShake()
    {        
        CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
    }
}
