using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOnInput : MonoBehaviour
{
    public float speed = 2f;

    private float speedLimit;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speedLimit = GetComponent<CsoundTransformAndPhysicsSender>().SpeedSender.maxSpeedValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Acccelerate();
        SpeedLimit();
    }

    void Acccelerate()
    {
        float horizontalInput = Input.GetAxis("Vertical");
        Vector3 forceVector = new Vector3(0, 0, horizontalInput);

        rb.AddForce(forceVector * speed, ForceMode.Acceleration);
    }

    void SpeedLimit()
    {
        if(rb.velocity.magnitude > speedLimit)
        {
            rb.velocity = rb.velocity.normalized * speedLimit;
        }
    }
}
