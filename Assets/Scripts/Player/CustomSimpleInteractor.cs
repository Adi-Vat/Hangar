using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomSimpleInteractor : MonoBehaviour
{
    [SerializeField]
    InputActionProperty gripAction;

    ModuleGripInteractable currentlySelectedModuleGrip;

    [SerializeField]
    Transform attachPoint;

    [SerializeField]
    float maximumInteractionDistance = 0.5f;

    [SerializeField]
    LayerMask interactableLayer;

    void OnEnable()
    {
        gripAction.action.Enable();
        // Callbacks for when the user presses the grip button
        gripAction.action.performed += OnGripPerformed;
        gripAction.action.canceled += OnGripCancelled;
    }

    private void OnDisable()
    {
        gripAction.action.performed -= OnGripPerformed;
        gripAction.action.canceled -= OnGripCancelled;

        gripAction.action.Disable();
    }

    void OnGripPerformed(InputAction.CallbackContext ctx)
    {
        TryGrab();
    }

    void OnGripCancelled(InputAction.CallbackContext ctx)
    {
        TryRelease();
    }

    void TryGrab()
    {
        Collider[] nearestColliders = Physics.OverlapSphere(attachPoint.position, maximumInteractionDistance, interactableLayer);

        float nearestDistance = -1;
        ModuleGripInteractable nearestModuleGrip = null;

        foreach (Collider collider in nearestColliders)
        {
            float distanceToCollider = Vector3.Distance(attachPoint.position, collider.ClosestPoint(attachPoint.position));

            if (nearestDistance == -1 || distanceToCollider < nearestDistance)
            {
                ModuleGripInteractable _modGrip = collider.GetComponent<ModuleGripInteractable>();
                if (_modGrip != null)
                {
                    nearestDistance = distanceToCollider;
                    nearestModuleGrip = _modGrip;
                }
            }
        }

        if (nearestModuleGrip == null) return;

        currentlySelectedModuleGrip = nearestModuleGrip;
        currentlySelectedModuleGrip.joystickBehaviour.hand = transform;
    }

    void TryRelease()
    {
        if(currentlySelectedModuleGrip == null) return;
        currentlySelectedModuleGrip.joystickBehaviour.hand = null;
        currentlySelectedModuleGrip = null;
    }
}
