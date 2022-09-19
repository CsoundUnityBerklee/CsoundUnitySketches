using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMagnitudeWithInput : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 boundaries;

    // Update is called once per frame
    void Update()
    {
        Scale();
        CheckBoundaries();
    }

    void Scale()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 scaleVector = new Vector3(horizontalInput, horizontalInput, horizontalInput);
        transform.localScale += scaleVector * speed * Time.deltaTime;
    }

    void CheckBoundaries()
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
