using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnStart : MonoBehaviour
{

    CsoundUnity csound;

    // Start is called before the first frame update
    void Start()
    {
        csound = GetComponent<CsoundUnity>();
        csound.SetChannel("gate", 1);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            csound.SetChannel("gate", 0);
        }
    }


}
