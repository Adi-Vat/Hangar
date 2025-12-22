using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;

    float interactButtonValue;
    Camera m_cam;

    [SerializeField]
    float interactionDistance;

    [SerializeField]
    LayerMask interactorLayerMask;

    Rigidbody rb;
    GravityBody gravityBody;
    PlayerGravity playerGravity;
    PlayerMovement playerMovement;

    [SerializeField]
    InputActionProperty interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = FindFirstObjectByType<PlayerInput>();
        m_cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        gravityBody = GetComponent<GravityBody>();
        playerGravity = GetComponent<PlayerGravity>();
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    private void OnEnable()
    {
        interactAction.action.actionMap.Enable();
        interactAction.action.performed += CheckInteraction; 
    }

    private void OnDisable()
    {
        interactAction.action.performed -= CheckInteraction;
        interactAction.action.Disable();
    }

    void CheckInteraction(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        if (Physics.Raycast(m_cam.transform.position, m_cam.transform.forward, out hit, interactionDistance, interactorLayerMask))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();

            if(interactable != null)
            {
                interactable.Interact(this);
            }
        }

        Debug.DrawRay(m_cam.transform.position, m_cam.transform.forward * interactionDistance, Color.red, 3f);
    }

    public void EnterShip(Transform pilotSeat)
    {
        transform.parent = pilotSeat.transform;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        m_cam.transform.localRotation = Quaternion.identity;
        playerMovement.pilotingShip = true;
        gravityBody.canAddForces = false;
    }

    public void ExitShip(Vector3 exitPosition)
    {
        transform.parent = null;
        transform.position = exitPosition;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        GetComponent<Collider>().enabled = true;
        playerMovement.pilotingShip = false;
        gravityBody.canAddForces = true;
    }
}
