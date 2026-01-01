using UnityEngine;

public class ShipThrusterManager : MonoBehaviour
{
    ThrusterBehaviour[] thrusters;
    ShipLocomotion shipLocomotion;

    ShipOutputComponent forwardOutput;
    ShipOutputComponent upOutput;
    ShipOutputComponent pitchOutput;
    ShipOutputComponent yawOutput;
    ShipOutputComponent rollOutput;

    [SerializeField]
    ShipComputer shipComputer;

    public float forwardOutputValue;
    public float upOutputValue;
    public float pitchOutputValue;
    public float yawOutputValue;
    public float rollOutputValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        shipLocomotion = GetComponentInParent<ShipLocomotion>();
        forwardOutput = shipComputer.forwardThruster;
        upOutput = shipComputer.upThruster;
        pitchOutput = shipComputer.pitchThruster;
        yawOutput = shipComputer.yawThruster;
        rollOutput = shipComputer.rollThruster;
        GetThrusters();
        ApplyThrusterValues();
    }

    private void Update()
    {
        forwardOutputValue = (forwardOutput != null) ? forwardOutput.inputValue : 0;
        upOutputValue = (upOutput != null) ? upOutput.inputValue : 0;
        pitchOutputValue = (pitchOutput != null) ? pitchOutput.inputValue : 0;
        yawOutputValue = (yawOutput != null) ? yawOutput.inputValue : 0;
        rollOutputValue = (rollOutput != null) ? rollOutput.inputValue : 0;
    }

    void GetThrusters()
    {
        thrusters = GetComponentsInChildren<ThrusterBehaviour>();
    }

    void ApplyThrusterValues()
    {
        float upPower = 0;
        float forwardPower = 0;
        Vector3 maxDeltaRotation = Vector3.zero;
        float pitchDecayTimeSeconds = 0;
        float yawDecayTimeSeconds = 0;
        float rollDecayTimeSeconds = 0;

        foreach (ThrusterBehaviour thruster in thrusters)
        {
            upPower += thruster.upPower;
            forwardPower += thruster.forwardPower;
            maxDeltaRotation += thruster.maxDeltaRotation;
            pitchDecayTimeSeconds += thruster.pitchDecayTimeSeconds;
            yawDecayTimeSeconds += thruster.yawDecayTimeSeconds;
            rollDecayTimeSeconds += thruster.rollDecayTimeSeconds;
        }

        shipLocomotion.upThrusterPower = upPower;
        shipLocomotion.forwardThrusterPower = forwardPower;
        shipLocomotion.maxDeltaRotation = maxDeltaRotation;
        shipLocomotion.maxPitchTime = pitchDecayTimeSeconds;
        shipLocomotion.maxYawTime = yawDecayTimeSeconds;
        shipLocomotion.maxRollTime = rollDecayTimeSeconds;
    }
}
