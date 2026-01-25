using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public float zoneWidth;
    public float zoneHeight;
    public float zoneLength;

    [SerializeField]
    Transform zoneGFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zoneGFX.transform.localScale = new Vector3(zoneWidth * 2, zoneHeight * 2, zoneLength * 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
