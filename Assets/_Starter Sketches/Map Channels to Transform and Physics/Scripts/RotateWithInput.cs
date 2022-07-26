using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithInput : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        RotateZAxis();
    }

    void RotateZAxis()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 rotationVector = new Vector3(0, 0, horizontalInput);
        gameObject.transform.Rotate(rotationVector * speed * Time.deltaTime);
    }
}
