using UnityEngine;
using EZCameraShake;

public class ShakeOnKeyPress : MonoBehaviour
{
    public float Magnitude = 2f;
    public float Roughness = 10f;
    public float FadeOutTime = 5f;

    void Update()
    {
        ThrustShake();
    }

    private void ThrustShake()
    {
        if (Input.GetAxis("Thrust") > 0)
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0, FadeOutTime);
    }
}
