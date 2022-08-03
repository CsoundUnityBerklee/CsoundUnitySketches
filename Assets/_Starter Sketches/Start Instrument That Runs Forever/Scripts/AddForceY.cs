using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceY : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float _force = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Ok");
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            rb.AddForce(transform.up * _force);
        }
    }
}
