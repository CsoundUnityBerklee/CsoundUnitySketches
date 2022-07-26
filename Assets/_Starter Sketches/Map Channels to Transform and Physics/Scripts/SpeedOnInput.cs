using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedOnInput : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Acccelerate();
    }

    void Acccelerate()
    {
        float horizontalInput = Input.GetAxis("Vertical");
        Vector3 forceVector = new Vector3(0, 0, horizontalInput);

        rb.AddForce(forceVector * speed, ForceMode.Acceleration);
    }
}
