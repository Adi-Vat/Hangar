using UnityEngine;

[RequireComponent(typeof(GravityBody))]
public class ShipGravity : MonoBehaviour
{
    GravityBody gravityBody;
    public float atmosphericAlignmentCorrectionCoefficient;

    private void Start()
    {
        gravityBody = GetComponent<GravityBody>();
    }

    void FixedUpdate()
    {
        CorrectRotation();
    }

    void CorrectRotation()
    {
        if (gravityBody.nearestAttractor == null) return;
        if (gravityBody.altitude < 0) return;

        Vector3 localUp = transform.up;
        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityBody.attractorNormal) * gravityBody.rb.rotation;

        float alignmentStrength = atmosphericAlignmentCorrectionCoefficient * (1 - gravityBody.altitude / gravityBody.nearestAttractor.atmoshpereRadius);

        gravityBody.rb.MoveRotation(
            Quaternion.Slerp(gravityBody.rb.rotation, targetRotation, alignmentStrength * Time.fixedDeltaTime)
            );
    }
}
