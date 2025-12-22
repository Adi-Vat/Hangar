using UnityEngine;

public class VelocityIndicatorBehaviour : MonoBehaviour
{
    [SerializeField]
    ShipOutputComponent xOutput;
    [SerializeField]
    ShipOutputComponent yOutput;
    [SerializeField]
    ShipOutputComponent zOutput;

    ShipLocomotion shipLocomotion;

    [SerializeField]
    Material whiteHologramMat;
    [SerializeField]
    Material redHologramMat;

    [SerializeField]
    LineRenderer localZ;
    [SerializeField]
    LineRenderer localX;
    [SerializeField]
    LineRenderer localY;

    Vector3 velocity;
    [SerializeField]
    Light m_light;

    [SerializeField]
    float arrowJitter;

    [SerializeField]
    Transform centreSphere;

    float zVelocitySharpness = 5;
    float yVelocitySharpness = 5;
    float xVelocitySharpness = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipLocomotion = GetComponentInParent<ShipLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(xOutput ? xOutput.inputValue : 0, yOutput ? yOutput.inputValue : 0, zOutput ? zOutput.inputValue : 0); ;
        velocity /= shipLocomotion.maxVelocity;

        velocity.z = (zVelocitySharpness*velocity.z / 1 + zVelocitySharpness*velocity.z);
        velocity.x = (xVelocitySharpness * velocity.x / 1 + xVelocitySharpness * velocity.x);
        velocity.y = (yVelocitySharpness * velocity.y / 1 + yVelocitySharpness * velocity.y);

        velocity *= 0.25f;

        float jitter = Random.Range(1f - arrowJitter, 1f);
        velocity *= jitter;

        m_light.intensity = 0.025f * jitter;
        centreSphere.localScale = Vector3.one * 0.025f * jitter;

        localZ.material = velocity.z >= 0 ? whiteHologramMat : redHologramMat;
        localX.material = velocity.x >= 0 ? whiteHologramMat : redHologramMat;
        localY.material = velocity.y >= 0 ? whiteHologramMat : redHologramMat;

        localZ.SetPosition(1, new Vector3(0, 0, velocity.z));
        localX.SetPosition(1, new Vector3(0, 0, velocity.x));
        localY.SetPosition(1, new Vector3(0, 0, velocity.y));
    }
}
