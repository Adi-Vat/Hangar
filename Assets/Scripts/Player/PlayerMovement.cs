using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Vector2 lookInput;

    Vector3 moveVector;
    float cameraPitch;
    float deltaYaw;
    float yaw;

    Rigidbody rb;

    [SerializeField]
    PlayerInput playerInput;

    PlayerGravity playerGravity;

    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float pitchLookSpeed;
    [SerializeField]
    float yawLookSpeed;

    Camera m_cam;

    float wraparoundTimer;
    bool outOfBounds;
    Vector3 wraparoundLocation;
    ZoneManager currentZone;

    [HideInInspector]
    public ShipLocomotion currentShip;

    [SerializeField]
    float zoneDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        m_cam = Camera.main;
        playerGravity = GetComponent<PlayerGravity>();

        currentZone = FindFirstObjectByType<ZoneManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        zoneDistance = (pos.x * pos.x)/(currentZone.zoneWidth * currentZone.zoneWidth) + (pos.y * pos.y)/(currentZone.zoneHeight * currentZone.zoneHeight) + (pos.z * pos.z)/(currentZone.zoneLength * currentZone.zoneLength);

        if (zoneDistance > 1) outOfBounds = true;
        else
        {
            outOfBounds = false;
            wraparoundTimer = 0;
        }


        lookInput = playerInput.actions["Look"].ReadValue<Vector2>();

        deltaYaw = lookInput.x * yawLookSpeed;

        yaw += lookInput.x * Time.deltaTime * yawLookSpeed;
        yaw = Mathf.Clamp(yaw, -140, 140);

        cameraPitch += -lookInput.y * Time.deltaTime * pitchLookSpeed;
        cameraPitch = Mathf.Clamp(cameraPitch, -70f, 70f);


        if (currentShip)
        {
            m_cam.transform.localRotation = Quaternion.Euler(cameraPitch, yaw, 0);
        }
        else
        {
            m_cam.transform.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
            moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
            moveVector = new Vector3(moveInput.x, 0, moveInput.y).normalized * moveSpeed;
        }

        
    }

    private void FixedUpdate()
    {
        if (!currentShip)
        {
            Vector3 gravityUp = playerGravity.gravityBody.attractorNormal;
            Vector3 forward = Vector3.ProjectOnPlane(rb.rotation * Vector3.forward, gravityUp).normalized;

            // Yaw input
            Quaternion yawRot = Quaternion.AngleAxis(deltaYaw * Time.fixedDeltaTime, gravityUp);

            Quaternion finalRotation = Quaternion.LookRotation(yawRot * forward, gravityUp);
            rb.MoveRotation(finalRotation);

            Vector3 localMove = transform.TransformDirection(moveVector * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + localMove);
        }

        if (outOfBounds)
        {
            if (wraparoundTimer == 0)
            {
                if (currentShip) wraparoundLocation = -currentShip.transform.position;
                else wraparoundLocation = -transform.position;
            }

            wraparoundTimer += Time.deltaTime;

            if (wraparoundTimer > 1)
            {
                WrapAround();
            }
        }
    }

    void WrapAround()
    {
        if (currentShip)
        {
            Vector3 vel = currentShip.rb.linearVelocity;
            currentShip.rb.isKinematic = true;
            currentShip.transform.position = wraparoundLocation;
            currentShip.rb.isKinematic = false;
            currentShip.rb.linearVelocity = vel;
        }
        else
        {
            //rb.isKinematic = true;
            //transform.position = wraparoundLocation;
            //rb.isKinematic = false;
        }

        outOfBounds = false;
    }
}
