using UnityEngine;

public class IndicatorLightBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject LEDObject;
    [SerializeField]
    Light LEDLight;

    [HideInInspector]
    public float LEDLightIntensityRatio;
    [SerializeField]
    float maxLEDLightIntensity;

    [SerializeField]
    Material offMateial;
    [SerializeField]
    Material onMaterial;

    [SerializeField]
    Color LEDColour;

    public void TurnOn()
    {
        MeshRenderer ledRenderer = LEDObject.GetComponent<MeshRenderer>();
        ledRenderer.material = onMaterial;
        ledRenderer.material.color = LEDColour;
        LEDLight.color = LEDColour;
        LEDLight.intensity = LEDLightIntensityRatio * maxLEDLightIntensity;
    }

    public void TurnOff()
    {
        MeshRenderer ledRenderer = LEDObject.GetComponent<MeshRenderer>();
        ledRenderer.material = offMateial;
        LEDLight.intensity = 0;
    }
}
