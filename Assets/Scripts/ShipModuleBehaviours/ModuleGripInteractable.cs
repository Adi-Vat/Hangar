using UnityEngine;

public class ModuleGripInteractable : MonoBehaviour
{
    public JoystickBehaviour joystickBehaviour { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joystickBehaviour = GetComponentInParent<JoystickBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
