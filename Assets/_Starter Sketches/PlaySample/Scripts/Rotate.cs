using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
   
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(20, 90, 20) * Time.deltaTime * _speed);
        
    }
}
