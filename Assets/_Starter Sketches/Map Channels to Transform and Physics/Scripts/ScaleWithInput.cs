using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithInput : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 boundaries;

    // Update is called once per frame
    void Update()
    {
        Scale();
        Boundaries();
    }

    void Scale()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float elevationInput = Input.GetAxis("Jump");

        Vector3 scaleVector = new Vector3(horizontalInput, elevationInput, verticalInput);

        transform.localScale += scaleVector * speed * Time.deltaTime;
    }

    void Boundaries()
    {
        if (transform.localScale.x > boundaries.x)
            transform.localScale = new Vector3(boundaries.x, transform.localScale.y, transform.localScale.z);
        else if (transform.localScale.x < -boundaries.x)
            transform.localScale = new Vector3(-boundaries.x, transform.localScale.y, transform.localScale.z);

        if (transform.localScale.y > boundaries.y)
            transform.localScale = new Vector3(transform.localScale.x, boundaries.y, transform.localScale.z);
        else if (transform.localScale.y < -boundaries.y)
            transform.localScale = new Vector3(transform.localScale.x, -boundaries.y, transform.localScale.z);

        if (transform.localScale.z > boundaries.z)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, boundaries.z);
        else if (transform.localScale.z < -boundaries.z)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -boundaries.z);
    }
}
