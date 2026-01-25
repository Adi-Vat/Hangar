using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField]
    float xAngle;
    [SerializeField]
    float zAngle;

    [SerializeField]
    IndicatorLightBehaviour[] X_Lights;
    [SerializeField]
    IndicatorLightBehaviour[] Y_Lights;

    [SerializeField]
    Vector3 directionToHand;

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
        
        Vector3 localHandPos = transform.InverseTransformPoint(hand.position);
        Vector3 localStickPos = transform.InverseTransformPoint(stick.position);
        
        Vector3 localDirToHand = (localHandPos - localStickPos).normalized;

        zAngle = 0;

        // Get angle between the forward direction to the hand and the relative up axis 
        // First convert the directions into 2D planes so we only look at one rotation axis
        Vector3 localUp = new Vector3(0, 1, 0);
        Vector3 localRight = new Vector3(1, 0 ,0);
        Vector3 forwardDirectionToHand = new Vector3(0, localDirToHand.y, localDirToHand.z);
        // get angle between z direction to hand and the local up direction.
        xAngle = -Vector3.SignedAngle(forwardDirectionToHand, localUp, localRight);

        xAngle = Mathf.Clamp(xAngle, -maxRotationDegrees, maxRotationDegrees);
        if (Mathf.Abs(xAngle) < deadZoneDegrees) xAngle = 0;

        if (joystickType == ControlInput.ControlInputValueOptions.TwoAxis)
        {
            Vector3 localForward = new Vector3(0, 0, 1);
            Vector3 rightDirectionToHand = new Vector3(localDirToHand.x, localDirToHand.y, 0);
            zAngle = Vector3.SignedAngle(rightDirectionToHand, localUp, localForward);

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
