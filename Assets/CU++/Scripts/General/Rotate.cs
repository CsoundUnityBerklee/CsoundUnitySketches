using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float _speed = 1f;
    [SerializeField] float X, Y, Z;
    [Range(1, 5)]
    [SerializeField] int scaling = 1;


    // Update is called once per frame
    void Update()
    {
        Rotation(X, Y, Z, _speed);

    }

    void Rotation(float x, float y, float z, float _speed)
    {
        this.transform.Rotate(new Vector3(x, y, z) * Time.deltaTime * _speed * scaling);
    }
}
