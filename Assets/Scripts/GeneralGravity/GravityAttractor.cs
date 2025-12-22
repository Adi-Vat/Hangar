using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float surfaceGravity;
    [HideInInspector]
    public float gravityStrength;
    public int surfaceRadius;
    public int atmoshpereRadius;
    public Color skyColour;
    public GameObject atmosphereObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer rend = atmosphereObject.GetComponent<MeshRenderer>();

        if (surfaceRadius == 0) return;
        transform.localScale = Vector3.one * surfaceRadius * 2;
        gravityStrength = surfaceGravity * (surfaceRadius * surfaceRadius);

        atmosphereObject.transform.localScale = Vector3.one * ((float)atmoshpereRadius/(float)surfaceRadius);

        rend.material.color = new Color(skyColour.r, skyColour.g, skyColour.b, 0.05f);
    }
}
