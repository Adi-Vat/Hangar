using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class GravityBody : MonoBehaviour
{
    GravityAttractor[] attractors;
    public GravityAttractor nearestAttractor;

    public Rigidbody rb;
    public Vector3 attractorNormal;
    public float altitude;
    public bool inAtmosphere;
    [SerializeField]
    float[] gravityStrengths;
    public bool canAddForces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attractors = FindObjectsByType<GravityAttractor>(FindObjectsSortMode.None);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        canAddForces = true;
        gravityStrengths = new float[attractors.Length];
    }

    GravityAttractor GetClosestAttractor()
    {
        GravityAttractor nearestAttractor = null;
        float nearestAttractorCorrectedGravity = -1;
        int i = 0;

        foreach(GravityAttractor attractor in attractors)
        {
            float r = Vector3.Distance(transform.position, attractor.transform.position);
            float thisAttractorCorrectedGravity = Mathf.Abs(attractor.gravityStrength / (r * r));

            if (thisAttractorCorrectedGravity < 0.001f) thisAttractorCorrectedGravity = 0;

            gravityStrengths[i] = thisAttractorCorrectedGravity;
            i++;

            if (thisAttractorCorrectedGravity > nearestAttractorCorrectedGravity)
            {
                nearestAttractor = attractor;
                nearestAttractorCorrectedGravity = thisAttractorCorrectedGravity;
            }
        }

        return nearestAttractor;
    }

    // Update is called once per frame
    void Update()
    {
        nearestAttractor = GetClosestAttractor();
        CalculateAltitude();
    }

    void FixedUpdate()
    {
        if (!canAddForces) return;
        AddGravityForce();
    }

    void AddGravityForce()
    {
        if (nearestAttractor == null) return;

        float distanceToAttractor = Vector3.Distance(transform.position, nearestAttractor.transform.position);
        attractorNormal = (transform.position - nearestAttractor.transform.position).normalized;
        float gravity = nearestAttractor.gravityStrength/(distanceToAttractor * distanceToAttractor);
        rb.AddForce(gravity * attractorNormal);
    }

    void CalculateAltitude()
    {
        float distanceToCentre = Vector3.Distance(transform.position, nearestAttractor.transform.position);

        // Only fix the rotation if the craft is inside the atmoshpere of the planet
        if (distanceToCentre > nearestAttractor.atmoshpereRadius)
        {
            inAtmosphere = false;
            altitude = -1;
            return;
        }

        inAtmosphere = true;
        altitude = distanceToCentre - nearestAttractor.surfaceRadius;
    }
}
