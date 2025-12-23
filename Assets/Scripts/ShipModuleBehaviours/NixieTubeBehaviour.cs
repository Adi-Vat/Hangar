using UnityEngine;

[RequireComponent(typeof(ShipOutputComponent))]
public class NixieTubeBehaviour : MonoBehaviour
{
    ShipOutputComponent outputComponent;

    int value;
    int oldValue;

    [SerializeField]
    Transform[] digits;
    [SerializeField]
    Transform negativeFlag;

    [SerializeField]
    GameObject[] zeroToNine;

    [SerializeField]
    Material onMaterial;
    [SerializeField]
    Material offMaterial;

    [SerializeField]
    float lightIntensity;

    private void Start()
    {
        outputComponent = GetComponent<ShipOutputComponent>();
        value = (int)outputComponent.inputValue;
        DrawTubes();
    }

    void Update()
    {
        value = (int)outputComponent.inputValue;
        if(value != oldValue) DrawTubes();
        oldValue = value;
    }

    void DrawTubes()
    {
        if (value == 0)
        {
            digits[0].GetComponentInChildren<MeshFilter>().mesh = zeroToNine[0].GetComponent<MeshFilter>().sharedMesh;
            SetLight(digits[0], true);
            return;
        }

        int[] digitNumbers = new int[digits.Length];
        // TODO: if the number of digits exceeds the number of nixie tubes, turn on the overflow flag
        
        if (value < 0) SetLight(negativeFlag, true);
        else SetLight(negativeFlag, false);

        for (int i = 0; i < digits.Length; i++)
        {
            int digitValue = Mathf.Abs((int)(value / Mathf.Pow(10, i)) % 10);
            digits[i].GetComponentInChildren<MeshFilter>().mesh = zeroToNine[digitValue].GetComponent<MeshFilter>().sharedMesh;
            digitNumbers[i] = digitValue;
        }

        bool numberStarted = false;

        for(int i = digits.Length - 1; i >= 0; i--)
        {
            if (digitNumbers[i] != 0) numberStarted = true;

            if (numberStarted) SetLight(digits[i], true);
            else SetLight(digits[i], false);
        }
    }

    void SetLight(Transform digit, bool on)
    {
        if (on)
        {
            digit.GetComponentInChildren<MeshRenderer>().material = onMaterial;
            digit.GetComponentInChildren<Light>().intensity = lightIntensity;
        }
        else
        {
            digit.GetComponentInChildren<MeshRenderer>().material = offMaterial;
            digit.GetComponentInChildren<Light>().intensity = 0;
        }
    }
}
