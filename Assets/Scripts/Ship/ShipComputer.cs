using UnityEngine;

public class ShipComputer : MonoBehaviour
{
    public Rigidbody shipRb;

    public ShipOutputComponent upThruster;
    public ShipOutputComponent forwardThruster;
    public ShipOutputComponent yawThruster;
    public ShipInputComponent shipVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipRb = transform.root.GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        GetVelocity();
    }

    void GetVelocity()
    {
        shipVelocity.output.value_vec3 = shipRb.transform.InverseTransformDirection(shipRb.linearVelocity);
    }
}
