using System.Collections.Generic;
using UnityEngine;

public class ShipModuleManager : MonoBehaviour
{
    [SerializeField]
    InputOutputConnector[] connectors;

    private void Update()
    {
        ApplyInputsToOutputs();
    }

    void ApplyInputsToOutputs()
    {
        foreach(InputOutputConnector connector in connectors)
        {
            if (connector.outputX != null)
            {
                connector.outputX.inputValue = connector.input.outputValue.x;
                Vector3 outputPort = connector.input.xOutputJack.position;
                Vector3 inputPort = connector.outputX.inputJack.position;

                LineRenderer cableRenderer = connector.input.xOutputJack.GetComponent<LineRenderer>();

                cableRenderer.SetPosition(0, outputPort);
                cableRenderer.SetPosition(1, inputPort);
            }
            if (connector.outputY != null)
            {
                connector.outputY.inputValue = connector.input.outputValue.y;
                Vector3 outputPort = connector.input.yOutputJack.position;
                Vector3 inputPort = connector.outputY.inputJack.position;

                LineRenderer cableRenderer = connector.input.yOutputJack.GetComponent<LineRenderer>();

                cableRenderer.SetPosition(0, outputPort);
                cableRenderer.SetPosition(1, inputPort);
            }
            if (connector.outputZ != null)
            {
                connector.outputZ.inputValue = connector.input.outputValue.z;
                Vector3 outputPort = connector.input.zOutputJack.position;
                Vector3 inputPort = connector.outputZ.inputJack.position;

                LineRenderer cableRenderer = connector.input.zOutputJack.GetComponent<LineRenderer>();

                cableRenderer.SetPosition(0, outputPort);
                cableRenderer.SetPosition(1, inputPort);
            }
        }
    }
}

[System.Serializable]
public class InputOutputConnector
{
    // Splits a Vector3 into 3 floats, so each axis can have a different use
    public ShipInputComponent input;
    public ShipOutputComponent outputX;
    public ShipOutputComponent outputY;
    public ShipOutputComponent outputZ;

    public InputOutputConnector(ShipInputComponent _input, ShipOutputComponent _outputX, ShipOutputComponent _outputY, ShipOutputComponent _outputZ)
    {
        input = _input;
        outputX = _outputX;
        outputY = _outputY;
        outputZ = _outputZ;
    }
}