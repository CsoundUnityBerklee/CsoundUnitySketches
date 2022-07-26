using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueOnInput : MonoBehaviour
{
    public float strength = 5f;
    private Rigidbody rb;
    private Vector3 torqueVector = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Torque();
        }
    }

    void Torque()
    {
        rb.AddTorque(torqueVector * strength);
    }
}
