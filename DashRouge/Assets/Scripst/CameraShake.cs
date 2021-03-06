using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    public Camera cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>();
        Shaking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            DoShake();
            Debug.Log("Collision occured");
        }
    }

    void FixedUpdate()
    {
        if (ShakeIntensity > 0)
        {
            cam.transform.position = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            
            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            Shaking = false;
        }
    }

    public void DoShake()
    {
        OriginalPos = cam.transform.position;

        ShakeIntensity = 3f;
        ShakeDecay = 0.5f;
        Shaking = true;
    }
}
