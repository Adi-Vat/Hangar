using UnityEngine;

public class ThrusterBehaviour : MonoBehaviour
{
    public float upPower;
    public float forwardPower;
    public Vector3 maxDeltaRotation;
    public float pitchDecayTimeSeconds;
    public float yawDecayTimeSeconds;
    public float rollDecayTimeSeconds;
    public Transform UpExhaustPlume;
    ShipLocomotion shipLocomotion;
    Light exhaustLight;

    private void Start()
    {
        shipLocomotion = GetComponentInParent<ShipLocomotion>();
        exhaustLight = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        float jitter = Random.Range(0.9f, 1.1f);
        UpExhaustPlume.localScale = new Vector3(1, shipLocomotion.acceleration.y / 3 * jitter, 1);
        exhaustLight.intensity = Mathf.Clamp(shipLocomotion.acceleration.y / 3 * jitter, 0, 1);
    }
}
