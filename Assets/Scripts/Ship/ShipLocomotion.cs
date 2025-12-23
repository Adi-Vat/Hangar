using NUnit.Framework;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipLocomotion : MonoBehaviour, IInteractable
{
    
    Vector3 shipRotationInput;
    // x: forward force, y: upwards force
    [SerializeField]
    Vector2 shipMovementInput;

    [HideInInspector]
    public Vector3 acceleration;
    Vector3 velocity;
    Vector3 damping;

    public float maxVelocity = 100;

    Vector3 deltaRotation;
    [HideInInspector]
    public Vector3 maxDeltaRotation;

    float yawAmount;
    float pitchAmount;
    float rollAmount;

    float yawTimer;
    float pitchTimer;
    float rollTimer;

    [HideInInspector]
    public float maxYawTime;
    [HideInInspector]
    public float maxPitchTime;
    [HideInInspector]
    public float maxRollTime;

    float yawMultiplier = 1;
    float pitchMultiplier = 1;
    float rollMultiplier = 1;

    int decayYawDirection;
    bool decayingYaw;
    float decayYawTimer;
    float maxYawReached;
    float maxDecayYawTimer;

    int decayPitchDirection;
    bool decayingPitch;
    float decayPitchTimer;
    float maxPitchReached;
    float maxDecayPitchTimer;

    int decayRollDirection;
    bool decayingRoll;
    float decayRollTimer;
    float maxRollReached;
    float maxDecayRollTimer;

    [HideInInspector]
    public float upThrusterPower;
    [HideInInspector]
    public float forwardThrusterPower;

    [SerializeField]
    float rotationSensitivity;

    PlayerInput playerInput;

    [SerializeField]
    bool isGrounded;
    [SerializeField]
    LayerMask attractorLayer;

    Collider[] colliders;

    Rigidbody rb;

    [SerializeField]
    bool beingPiloted;

    [SerializeField]
    Transform pilotSeat;

    [SerializeField]
    Transform shipExit;

    GravityBody gravityBody;

    float forwardVelocityDamping = 0.1f;
    float upwardsVelocityDamping = 3f;

    ShipThrusterManager shipThrusterManager;

    int fuel = 100;

    [SerializeField]
    ShipInputComponent shipFuelInputComponent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
        rb = GetComponent<Rigidbody>();
        playerInput = FindFirstObjectByType<PlayerInput>();
        gravityBody = GetComponent<GravityBody>();
        shipThrusterManager = GetComponentInChildren<ShipThrusterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (beingPiloted)
        {
            shipRotationInput = playerInput.actions["ShipRotation"].ReadValue<Vector3>();
            shipMovementInput = playerInput.actions["ShipMovement"].ReadValue<Vector2>();
            //shipMovementInput = new Vector2(shipThrusterManager.forwardOutputValue, shipThrusterManager.upOutputValue);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            CalculateNewVelocity();
            CalculateNewRotation();
            UpdateFuel();
        }
        else
        {
            shipRotationInput = Vector3.zero;
            shipMovementInput = Vector3.zero;
            if (isGrounded) rb.constraints = RigidbodyConstraints.FreezeAll;
            else rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        CheckGrounded();
    }

    void UpdateFuel()
    {
        fuel = Mathf.RoundToInt(50 * Mathf.Sin(Time.time / 10)) + 50;
        shipFuelInputComponent.output.value_float = fuel;
    }

    void CheckGrounded()
    {
        if (gravityBody.nearestAttractor == null) return;

        // Go through each collider
        // Raycast from collider centre to centre of planet
        // Get intersection point on surface of planet
        // Get the closest point on the collider to the surface
        // Check distance from closest collider point to surface
        // Get smallest distance and therefore closest collider point
        // Check ground on this point

        Vector3 nearestPointToGround = transform.position;
        float distanceToBeat = -1;
        Vector3 surfacePoint = transform.position;

        foreach(Collider col in colliders)
        {
            Vector3 colliderCentre = col.bounds.center;
            Vector3 dirToGround = (gravityBody.nearestAttractor.transform.position - colliderCentre).normalized;

            RaycastHit hit;
            if(Physics.Raycast(colliderCentre, dirToGround, out hit, Mathf.Infinity, attractorLayer))
            {
                Vector3 colliderPointNearGround = col.ClosestPoint(hit.point);

                float distanceToGround = Vector3.Distance(colliderPointNearGround, hit.point);
                if(distanceToGround < distanceToBeat || distanceToBeat == -1)
                {
                    distanceToBeat = distanceToGround;
                    nearestPointToGround = colliderPointNearGround;
                    surfacePoint = hit.point;
                }
            }
        }
        Debug.DrawLine(nearestPointToGround, surfacePoint, Color.red);
        isGrounded = Physics.CheckSphere(nearestPointToGround, 0.25f, attractorLayer);
    }

    private void FixedUpdate()
    {
        if (!beingPiloted) return;



        /*
        float falloffStrength = 10;
        float forwardVelocityFalloffApplied = maxVelocity * falloffStrength * velocity.z / (maxVelocity + falloffStrength * falloffStrength);
        */


        rb.AddRelativeForce(acceleration + damping);
        velocity = transform.InverseTransformDirection(rb.linearVelocity);

        deltaRotation = new Vector3(-pitchAmount, yawAmount, rollAmount) * Time.fixedDeltaTime * rotationSensitivity;

        Quaternion targetRotation = rb.rotation * Quaternion.Euler(deltaRotation);
        rb.MoveRotation(targetRotation);

        //   Quaternion targetRotation = rb.rotation * Quaternion.Euler(new Vector3(-shipRotationInput.y, shipRotationInput.x, shipRotationInput.z) * Time.fixedDeltaTime * yThrusterSensitivity);


        //Quaternion newRotation = rb.rotation * Quaternion.Euler(new Vector3(-shipRotationInput.y, shipRotationInput.x, shipRotationInput.z) * Time.fixedDeltaTime * yThrusterSensitivity);
        //rb.MoveRotation(newRotation);
        //rb.MoveRotation();
        //rb.AddRelativeTorque(new Vector3(-shipRotationInput.y, shipRotationInput.x, shipRotationInput.z) * yThrusterSensitivity);
        
        //rb.AddRelativeForce(new Vector3(0, shipMovementInput.y, shipMovementInput.x), ForceMode.Force);
        /*
        Vector3 velocity = rb.linearVelocity;
        Vector3 forwardVelocity = Vector3.Project(velocity, transform.forward);
        Vector3 lateralVelocity = velocity - forwardVelocity;
        float assistStrength = 0.3f;
        Vector3 correctionForce = -lateralVelocity * assistStrength;
        rb.AddForce(correctionForce, ForceMode.Acceleration);
    */
    }

    void CalculateNewRotation()
    {
        // Rotational decay
        // Once the player lets go of the turn button
        // Keep rotating the ship
        // Decay this rotation over time
        // Until the ship stops rotating


        // If the player is trying to rotate the ship
        // Slowly increase the speed of rotation from 0 to max
        if (Mathf.Abs(shipRotationInput.x) > 0)
        {
            decayYawDirection = (int)Mathf.Sign(shipRotationInput.x);
            // The user is now in control of the yaw amount
            decayingYaw = false;
            // Gets value between 0 and max angular velocity based on the time the button has been held
            yawMultiplier = Mathf.Lerp(0, maxDeltaRotation.y, yawTimer / maxYawTime);
            if (yawTimer < maxYawTime) yawTimer += Time.fixedDeltaTime;
            else yawTimer = maxYawTime;
        }
        else // If the button isn't being pressed
        {
            // The user's yaw input is 0, so the ship doesn't rotate, but the yaw multiplier is whatever the user just had
            // And the user just let go
            if (yawTimer > 0)
            {
                maxDecayYawTimer = yawTimer;
                decayYawTimer = 0;
                maxYawReached = yawMultiplier;
                // Keep the spin, the computer is now in control of the yaw amount
                decayingYaw = true;
            }

            // The user has now let go
            yawTimer = 0;
        }

        if (decayingYaw)
        {
            yawMultiplier = Mathf.Lerp(maxYawReached, 0, decayYawTimer / maxDecayYawTimer);

            if (decayYawTimer < maxDecayYawTimer) decayYawTimer += Time.fixedDeltaTime;
            else
            {
                decayYawTimer = maxDecayYawTimer;
                decayingYaw = false;
            }
        }

        if (Mathf.Abs(shipRotationInput.y) > 0)
        {
            decayPitchDirection = (int)Mathf.Sign(shipRotationInput.y);
            // The user is now in control of the pitch amount
            decayingPitch = false;
            // Gets value between 0 and max angular velocity based on the time the button has been held
            pitchMultiplier = Mathf.Lerp(0, maxDeltaRotation.x, pitchTimer / maxPitchTime);
            if (pitchTimer < maxPitchTime) pitchTimer += Time.fixedDeltaTime;
            else pitchTimer = maxPitchTime;
        }
        else // If the button isn't being pressed
        {
            // The user's yaw input is 0, so the ship doesn't rotate, but the yaw multiplier is whatever the user just had
            // And the user just let go
            if (pitchTimer > 0)
            {
                maxDecayPitchTimer = pitchTimer;
                decayPitchTimer = 0;
                maxPitchReached = pitchMultiplier;
                // Keep the spin, the computer is now in control of the yaw amount
                decayingPitch = true;
            }

            // The user has now let go
            pitchTimer = 0;
        }

        if (decayingPitch)
        {
            pitchMultiplier = Mathf.Lerp(maxPitchReached, 0, decayPitchTimer / maxDecayPitchTimer);

            if (decayPitchTimer < maxDecayPitchTimer) decayPitchTimer += Time.fixedDeltaTime;
            else
            {
                decayPitchTimer = maxDecayPitchTimer;
                decayingPitch = false;
            }
        }


        if (Mathf.Abs(shipRotationInput.z) > 0)
        {
            decayRollDirection = (int)Mathf.Sign(shipRotationInput.z);
            // The user is now in control of the pitch amount
            decayingRoll = false;
            // Gets value between 0 and max angular velocity based on the time the button has been held
            rollMultiplier = Mathf.Lerp(0, maxDeltaRotation.z, rollTimer / maxRollTime);
            if (rollTimer < maxRollTime) rollTimer += Time.fixedDeltaTime;
            else rollTimer = maxRollTime;
        }
        else // If the button isn't being pressed
        {
            // The user's yaw input is 0, so the ship doesn't rotate, but the yaw multiplier is whatever the user just had
            // And the user just let go
            if (rollTimer > 0)
            {
                maxDecayRollTimer = rollTimer;
                decayRollTimer = 0;
                maxRollReached = rollMultiplier;
                // Keep the spin, the computer is now in control of the yaw amount
                decayingRoll = true;
            }

            // The user has now let go
            rollTimer = 0;
        }

        if (decayingRoll)
        {
            rollMultiplier = Mathf.Lerp(maxRollReached, 0, decayRollTimer / maxDecayRollTimer);

            if (decayRollTimer < maxDecayRollTimer) decayRollTimer += Time.fixedDeltaTime;
            else
            {
                decayRollTimer = maxDecayRollTimer;
                decayingRoll = false;
            }
        }

        // If the user has let go, and the yaw should decay, apply the correct decay amount
        yawAmount = (decayingYaw ? decayYawDirection : shipRotationInput.x) * yawMultiplier;
        pitchAmount = (decayingPitch ? decayPitchDirection : shipRotationInput.y) * pitchMultiplier;
        rollAmount = (decayingRoll ? decayRollDirection : shipRotationInput.z) * rollMultiplier;
    }

    void CalculateNewVelocity()
    {
        // ROTATION
        // Pitch, yaw, roll
        // Takes ~0.3-0.5s to reach max rotation speed
        // Rotation speed caps out
        // Takes some time to return to a state of no rotation

        // MOVEMENT
        // Forward/back thruster, up/down thruster, strafing thrusters
        // Forward/back thruster is purely accelerational, capping out at some max speed
        // When let go of, forward velocity slows to 0 after 5-10 seconds?
        // Up/down thruster also accelerational at full power during takeoff/landing (in atmosphere)
        // but operate at ~30% during space flight
        // up velocity (during spaceflight) slows to 0 after 3-5s?
        // strafing thrusters immediate response
        // no inertia, for docking only.
        // tied to velocity, high velocity = low strafing power

        // Makes it impossible to thrust forward if on the ground
        float forwardForce = shipMovementInput.x * forwardThrusterPower * (isGrounded ? 0 : 1);
        // Makes up (takeoff) thrusters fire at 30% in space, and makes getting the first bit off the ground the hardest (halfs thruster power)
        float upwardsForce = shipMovementInput.y * upThrusterPower * (gravityBody.inAtmosphere ? 1 : 0.3f); //* (isGrounded ? 0.5f : 1);

        // If the ship exceeds the speed limit, no more force may be applied
        if (velocity.z > maxVelocity)
        {
            if (forwardForce > 0) forwardForce = 0;
        }
        else if (velocity.z < -maxVelocity)
        {
            if (forwardForce < 0) forwardForce = 0;
        }

        if (velocity.y > maxVelocity)
        {
            if (upwardsForce > 0) upwardsForce = 0;
        }
        else if (velocity.y < -maxVelocity)
        {
            if (upwardsForce < 0) upwardsForce = 0;
        }

        acceleration = new Vector3(0, upwardsForce, forwardForce);
        damping = Vector3.zero;

        // If no thrust is being given, slowly attenuate the velocities
        // maybe replace with v_new = v_old * e^(-k dt) if this model becomes jittery around v = 0
        if (forwardForce == 0) damping.z = -velocity.z * forwardVelocityDamping;
        if (upwardsForce == 0) damping.y = -velocity.y * (gravityBody.inAtmosphere ? 0 : upwardsVelocityDamping);
    }

    public void EnterShip(PlayerInteraction player)
    {
        player.EnterShip(pilotSeat);
        
    }

    public void ExitShip(PlayerInteraction player)
    {
        player.ExitShip(shipExit.position);
    }

    public void Interact(PlayerInteraction player)
    {
        beingPiloted = !beingPiloted;
        if (beingPiloted) EnterShip(player);
        else ExitShip(player);
    }
}
