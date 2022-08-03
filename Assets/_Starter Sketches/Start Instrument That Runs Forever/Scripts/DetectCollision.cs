using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{

    CsoundUnity csound;

    // Start is called before the first frame update
    void Start()
    {
        csound = GetComponent<CsoundUnity>();    
    }

    private void OnTriggerEnter(Collider other)
    {
        csound.SetChannel("gate", 0);
    }

    private void OnTriggerExit(Collider other)
    {
        csound.SetChannel("gate", 1);
    }


}
