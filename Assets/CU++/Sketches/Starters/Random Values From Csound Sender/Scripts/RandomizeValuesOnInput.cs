using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeValuesOnInput : MonoBehaviour
{
    private CsoundSender csoundSender;

    // Start is called before the first frame update
    void Start()
    {
        csoundSender = GetComponent<CsoundSender>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            csoundSender.SetChannelsToRandomValue();
        }
    }
}
