using UnityEngine;

[RequireComponent(typeof(GravityBody))]
public class PlayerGravity : MonoBehaviour
{
    public GravityBody gravityBody;
    Camera m_cam;

    Color spaceColour;
    Color planetSkyColour;

    private void Start()
    {
        gravityBody = GetComponent<GravityBody>();
        m_cam = Camera.main;
        spaceColour = m_cam.backgroundColor;
    }

    private void Update()
    {
        CalculateSkyColour();
    }

    void CalculateSkyColour()
    {
        if(gravityBody.altitude > 0)
        {
            m_cam.clearFlags = CameraClearFlags.Color;
            planetSkyColour = gravityBody.nearestAttractor.skyColour;
            float colourLerpAmount = 1 - gravityBody.altitude / (gravityBody.nearestAttractor.atmoshpereRadius - gravityBody.nearestAttractor.surfaceRadius);
            m_cam.backgroundColor = Color.Lerp(spaceColour, planetSkyColour, colourLerpAmount);
            RenderSettings.fog = true;
            RenderSettings.fogColor = planetSkyColour;
            RenderSettings.fogDensity = colourLerpAmount * 0.002f;
            gravityBody.nearestAttractor.atmosphereObject.SetActive(false);
        }
        else
        {
            if (gravityBody.nearestAttractor != null) gravityBody.nearestAttractor.atmosphereObject.SetActive(true);

            m_cam.backgroundColor = spaceColour;
            m_cam.clearFlags = CameraClearFlags.Skybox;
            RenderSettings.fog = false;
        }

    }

    public Quaternion CorrectRotation()
    {
        Vector3 localUp = transform.up;
        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityBody.attractorNormal) * gravityBody.rb.rotation;
        return targetRotation;
    }
}
