using UnityEngine;

public class ShipThrusterManager : MonoBehaviour
{
    ThrusterBehaviour[] thrusters;
    ShipLocomotion shipLocomotion;

    ShipOutputComponent forwardOutput;
    ShipOutputComponent upOutput;
    ShipOutputComponent yawOutput;

    [SerializeField]
    ShipComputer shipComputer;

    public float forwardOutputValue;
    public float upOutputValue;
    public float yawOutputValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        shipLocomotion = GetComponentInParent<ShipLocomotion>();
        forwardOutput = shipComputer.forwardThruster;
        upOutput = shipComputer.upThruster;
        yawOutput = shipComputer.yawThruster;
        GetThrusters();
        ApplyThrusterValues();
    }

    private void Update()
    {
        forwardOutputValue = (forwardOutput != null) ? forwardOutput.inputValue : 0;
        upOutputValue = (upOutput != null) ? upOutput.inputValue : 0;
        yawOutputValue = (yawOutput != null) ? yawOutput.inputValue : 0;
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
