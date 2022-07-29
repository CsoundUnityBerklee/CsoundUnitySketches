using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueOnInput : MonoBehaviour
{
    public float strength = 5f;

    private float torqueLimit;
    private Rigidbody rb;
    private Vector3 torqueVector = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        torqueLimit = GetComponent<CsoundTransformAndPhysicsSender>().AngularSpeedSender.maxAngularSpeedValue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Torque();
        }

        TorqueLimit();
    }

    void Torque()
    {
        rb.AddTorque(torqueVector * strength);
    }

    void TorqueLimit()
    {
        if(rb.angularVelocity.magnitude > torqueLimit)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * torqueLimit;
        }
    }
}
