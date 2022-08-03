using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalInstrumentOnStart : MonoBehaviour
{
    CsoundUnity csound;
    // Start is called before the first frame update
    void Start()
    {
        csound = GetComponent<CsoundUnity>();
        csound.SetChannel("gate", 1);
        
    }
}
