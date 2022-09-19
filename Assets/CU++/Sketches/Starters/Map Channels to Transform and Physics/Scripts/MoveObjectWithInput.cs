using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectWithInput : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 boundaries;

    // Update is called once per frame
    void Update()
    {
        MoveObject();
        CheckBoundaries();
    }

    void MoveObject()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float elevationInput = Input.GetAxis("Jump");

        Vector3 movementVector = new Vector3(horizontalInput, elevationInput, verticalInput);
        transform.Translate(movementVector * speed * Time.deltaTime);
    }

    void CheckBoundaries()
    {
        if (transform.position.x > boundaries.x)
            transform.position = new Vector3(boundaries.x, transform.position.y, transform.position.z);
        else if(transform.position.x < -boundaries.x)
            transform.position = new Vector3(-boundaries.x, transform.position.y, transform.position.z);

        if (transform.position.y > boundaries.y)
            transform.position = new Vector3(transform.position.x, boundaries.y, transform.position.z);
        else if (transform.position.y < -boundaries.y)
            transform.position = new Vector3(transform.position.x, -boundaries.y, transform.position.z);

        if (transform.position.z > boundaries.z)
            transform.position = new Vector3(transform.position.x, transform.position.y, boundaries.z);
        else if (transform.position.z < -boundaries.z)
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundaries.z);
    }
}