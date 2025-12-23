using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(ShipInputComponent))]
public class JoystickBehaviour : MonoBehaviour
{
    ShipInputComponent shipInputComponent;
    ControlInput.ControlInputValueOptions joystickType;

    [SerializeField]
    Vector2 output;
    [SerializeField]
    Transform stick;

    [SerializeField]
    float maxRotationDegrees;

    [SerializeField]
    float deadZoneDegrees;

    [SerializeField]
    bool thisGrasped;
    [SerializeField]
    Transform hand;

    //[SerializeField]
    //ControlInput.ControlInputValueOptions outputValueType;

    float xAngle;
    float zAngle;

    [SerializeField]
    IndicatorLightBehaviour[] X_Lights;
    [SerializeField]
    IndicatorLightBehaviour[] Y_Lights;

    int middleLightIndexX;
    int middleLightIndexY;

    private void Awake()
    {
        shipInputComponent = GetComponent<ShipInputComponent>();
        //shipInputComponent.output.outputType = outputValueType;
        middleLightIndexY = (int)(Y_Lights.Length / 2);
        middleLightIndexX = (int)(X_Lights.Length / 2);
        joystickType = shipInputComponent.outputType;
    }

    void Update()
    {
        if (!thisGrasped || hand == null) return;

        // Draw a line between the base of the joystick and the hand that's holding it
        // Gets a direction that the joystick should be pointing in
        // Get the rotation required to get to this direction
        // Rotate the joystick accordingly with clamping
        Vector3 directionToHand = (hand.position - stick.transform.position).normalized;

        zAngle = 0;
        
        xAngle = Vector2.SignedAngle(new Vector2(directionToHand.z, directionToHand.y), new Vector2(transform.up.z, transform.up.y));
        xAngle = Mathf.Clamp(xAngle, -maxRotationDegrees, maxRotationDegrees);
        if (Mathf.Abs(xAngle) < deadZoneDegrees) xAngle = 0;

        if (joystickType == ControlInput.ControlInputValueOptions.TwoAxis)
        {
            zAngle = Vector2.SignedAngle(new Vector2(directionToHand.x, directionToHand.y), new Vector2(transform.up.x, transform.up.y));
            zAngle = Mathf.Clamp(zAngle, -maxRotationDegrees, maxRotationDegrees);
            if (Mathf.Abs(zAngle) < deadZoneDegrees) zAngle = 0;

            output.y = Mathf.Clamp(xAngle / maxRotationDegrees, -1, 1);
            output.x = Mathf.Clamp(zAngle / maxRotationDegrees, -1, 1);
            shipInputComponent.output.value_vec2 = output;
        }
        else
        {
            output.x = Mathf.Clamp(xAngle / maxRotationDegrees, -1, 1);
            shipInputComponent.output.value_float = output.x;
        }

        stick.localRotation = Quaternion.Euler(new Vector3(xAngle, 0, -zAngle));

        ApplyIndicatorLights();
    }

    void ApplyIndicatorLights()
    { 
        for (int i = 0; i < X_Lights.Length; i++)
        {
            X_Lights[i].TurnOff();
        }

        X_Lights[middleLightIndexX].TurnOn();

        // X Direction
        int XledIndex = Mathf.RoundToInt(output.x * middleLightIndexX);
        XledIndex += middleLightIndexX;

        if (XledIndex < middleLightIndexX)
        {
            for (int i = middleLightIndexX; i >= XledIndex; i--)
            {
                X_Lights[i].TurnOn();
            }
        }
        else if (XledIndex > middleLightIndexX)
        {
            for (int i = middleLightIndexX; i <= XledIndex; i++)
            {
                X_Lights[i].TurnOn();
            }
        }

        if (joystickType == ControlInput.ControlInputValueOptions.OneAxis) return;

        for (int i = 0; i < Y_Lights.Length; i++)
        {
            Y_Lights[i].TurnOff();
        }

        Y_Lights[middleLightIndexY].TurnOn();

        // Y Direction
        int YledIndex = Mathf.RoundToInt(output.y * middleLightIndexY);
        YledIndex += middleLightIndexY;

        if (YledIndex < middleLightIndexY)
        {
            for (int i = middleLightIndexY; i >= YledIndex; i--)
            {
                Y_Lights[i].TurnOn();
            }
        }
        else if (YledIndex > middleLightIndexY)
        {
            for (int i = middleLightIndexY; i <= YledIndex; i++)
            {
                Y_Lights[i].TurnOn();
            }
        }
    }

    Vector3 UnsignedToSignedEulerAngles(Vector3 unsignedAngle)
    {
        Vector3 signedAngle = unsignedAngle;

        if (unsignedAngle.x > 180) signedAngle.x = -360 + unsignedAngle.x;
        if (unsignedAngle.z > 180) signedAngle.z = -360 + unsignedAngle.z;
        if (unsignedAngle.z > 180) signedAngle.z = -360 + unsignedAngle.z;

        return signedAngle;
    }

    public void Grasp(Transform _hand)
    {
        hand = _hand;
        thisGrasped = true;
    }
}
