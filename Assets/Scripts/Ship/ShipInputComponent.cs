using Unity.VisualScripting;
using UnityEngine;

public class ShipInputComponent : MonoBehaviour
{
    [HideInInspector]
    public ControlInput output;
    public ControlInput.ControlInputValueOptions outputType;

    public Vector3 outputValue { get; private set; }

    public Transform xOutputJack;
    public Transform yOutputJack;
    public Transform zOutputJack;

    private void Awake()
    {
        output.outputType = outputType;
    }

    private void Update()
    {
        outputValue = GetOutputValue();
    }

    public Vector3 GetOutputValue()
    {
        Vector3 returnValue = Vector3.zero;

        switch (output.outputType)
        {
            case ControlInput.ControlInputValueOptions.Button:
                returnValue = new Vector3(output.value_bool ? 1:0, 0, 0);
                break;
            case ControlInput.ControlInputValueOptions.OneAxis:
                returnValue = new Vector3(output.value_float, 0, 0);
                break;
            case ControlInput.ControlInputValueOptions.TwoAxis:
                returnValue = new Vector3(output.value_vec2.x, output.value_vec2.y, 0);
                break;
            case ControlInput.ControlInputValueOptions.ThreeAxis:
                returnValue = output.value_vec3;
                break;
        }

        return returnValue;
    }
}

[System.Serializable]
public class ControlInput
{
    public enum ControlInputValueOptions
    {
        Button,
        OneAxis,
        TwoAxis,
        ThreeAxis
    }

    public ControlInputValueOptions outputType;

    public bool value_bool { get; set; }
    public float value_float { get; set; }
    public Vector2 value_vec2 { get; set; }
    public Vector3 value_vec3 { get; set; }

    public ControlInput(ControlInputValueOptions _outputType)
    {
        outputType = _outputType;
    }
}
